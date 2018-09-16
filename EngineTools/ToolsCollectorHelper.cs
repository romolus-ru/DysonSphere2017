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
	internal class ToolsCollectorHelper
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

		private static List<string> _dllIgnored = new List<string>() {
				"EntityFramework.dll",  "EntityFramework.SqlServer.dll",
				"Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll",
				"UnitTests.dll", "OpenGL4Net.dll", "Tao.DevIl.dll",
				"Tao.FreeGlut.dll", "Tao.Platform.Windows.dll",
		};

		/// <summary>
		/// Получаем имена файлов которые можно просканировать
		/// </summary>
		/// <returns></returns>
		public static List<string> GetFiles()
		{
			var appPath = StateEngine.AppPath;
			var files = Directory.GetFiles(appPath, "*.dll");
			var ret = new List<string>();
			foreach (var fl in files) {
				var str = _dllIgnored.Where(s => fl.Contains(s)).FirstOrDefault();
				if (string.IsNullOrEmpty(str)) continue;
				ret.Add(fl);
			}
			files = Directory.GetFiles(appPath, "*.exe");
			foreach (var fl in files) {
				if (fl.EndsWith(".vshost.exe", StringComparison.InvariantCultureIgnoreCase)) continue;
				ret.Add(fl);
			}
			return ret;
		}

		public static Type GetTypeFromFile(string classFile, string className)
		{
			var types = SearchObjectsInAssembly(classFile, typeof(object));
			foreach (var tp in types) {
				if (tp.FullName == className)
					return tp;
			}
			return null;
		}

		public static List<string> GetClassesInFile(string fileName)
		{
			var ret = new List<string>();
			var cl = SearchObjectsInAssembly(fileName, typeof(object));
			foreach (var item in cl) {
				ret.Add(item.FullName);
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