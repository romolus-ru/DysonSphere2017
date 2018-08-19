using Engine;
using Engine.Data;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EngineTools
{
	/// <summary>
	/// Поиск файлов-сборок и типов в них
	/// </summary>
	internal class ToolsCollectorManager
	{
		public static List<CollectClass> GetCollectClasses<T>(DataSupportBase ds) where T : class
		{
			Collector collector = StateEngine.Collector;
			if (collector == null) {
				StateEngine.Log.AddLog("Collector is null");
				return null;
			}
			var baseType = typeof(T);
			var ids = collector.GetRegisteredSubClasses(baseType);
			if (ids.Count == 0) return null;

			var ret = new List<CollectClass>();
			foreach (var ccl in ds.GetCollectClasses()) {
				if (ids.Contains(ccl.Id)) ret.Add(ccl);
			}
			return ret;
		}

		public static List<CollectClass> GetAllClasses<T>(DataSupportBase ds) where T : class
		{
			var ret = GetCollectClasses<T>(ds);
			if (ret == null)
				ret = new List<CollectClass>();

			var files = GetFiles();
			var baseType = typeof(T);
			var appPath = StateEngine.AppPath;
			foreach (var fileName in files) {
				var types = SearchObjectsInAssembly(fileName, baseType);
				if (types == null || types.Count == 0) continue;

				var shortFileName = fileName.Substring(appPath.Length);
				foreach (var ccl in ret) {
					if (ccl.FileName != shortFileName) continue;
					var type1 = types.FirstOrDefault(t => t.FullName == ccl.ClassName);
					if (type1 == null) continue;// не нашли такой класс
					types.Remove(type1);// нашли такой класс, уже зарегистрирован - удаляем
				}
				if (types.Count == 0) continue;
				foreach (var tp in types) {
					var ccl = new CollectClass();
					ccl.FileName = shortFileName;
					ccl.ClassName = tp.FullName;
					ret.Add(ccl);
				}
			}
			return ret;
		}

		/// <summary>
		/// Получаем имена файлов которые надо просканировать
		/// </summary>
		/// <returns></returns>
		private static List<string> GetFiles()
		{
			var appPath = StateEngine.AppPath;
			var files = Directory.GetFiles(appPath, "*.dll");
			var ret = new List<string>();
			foreach (var fl in files) {
				if (fl.Contains("EntityFramework.dll")) continue;
				if (fl.Contains("EntityFramework.SqlServer.dll")) continue;
				if (fl.Contains("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")) continue;
				if (fl.Contains("UnitTests.dll")) continue;
				if (fl.Contains("OpenGL4Net.dll")) continue;
				if (fl.Contains("Tao.DevIl.dll")) continue;
				if (fl.Contains("Tao.FreeGlut.dll")) continue;
				if (fl.Contains("Tao.Platform.Windows.dll")) continue;
				ret.Add(fl);
			}
			return ret;
		}

		private static List<Type> SearchObjectsInAssembly(string assemblyFile, Type baseType)
		{
			Assembly assembly;
			if (!File.Exists(assemblyFile)) return null;
			try {// пробуем загрузить
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e) {// если не загрузилось то показываем что к чему
				StateEngine.Log?.AddLog("Ошибка при загрузке сборки " + assemblyFile + Environment.NewLine + e.Message + Environment.NewLine + e.GetType());
				return null; // и выходим
			}
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			var ftypeName = "." + baseType;
			Type[] types = assembly.GetTypes();
			var ret = new List<Type>();
			foreach (Type type in types) {
				if (type.FullName.Contains("<")) continue;
				if (type.FullName.Contains("+")) continue;
				if (type.FullName.Contains("`")) continue;
				if (!type.IsSubclassOf(baseType)) continue;
				ret.Add(type);
			}
			return ret;
		}

		public static void SaveNewCollectorClass(DataSupportBase ds, CollectClass collectClass)
		{
			if (collectClass.Id > 0)
				return;
			ds.SaveCollectClasses(collectClass);
			Collector collector = StateEngine.Collector;
			if (collector == null) return;
			collector.LoadClasses(new List<CollectClass> { collectClass });
		}
	}
}