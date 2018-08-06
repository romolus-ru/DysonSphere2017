using Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EngineTools
{
	/// <summary>
	/// Поиск файлов-сборок и типов в них
	/// </summary>
	internal class ToolsCollectorManager
	{
		private void ScanFiles(DataSupportBase ds, string typeName)
		{
			var appPath = AppDomain.CurrentDomain.BaseDirectory;
			var files = Directory.GetFiles(appPath, "*.dll");
			// получаем классы из файлов
			foreach (var fl in files) {
				if (fl.Contains("EntityFramework.dll")) continue;
				if (fl.Contains("EntityFramework.SqlServer.dll")) continue;
				if (fl.Contains("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")) continue;
				if (fl.Contains("UnitTests.dll")) continue;
				if (fl.Contains("OpenGL4Net.dll")) continue;
				if (fl.Contains("Tao.DevIl.dll")) continue;
				if (fl.Contains("Tao.FreeGlut.dll")) continue;
				if (fl.Contains("Tao.Platform.Windows.dll")) continue;
				FindObjectsInAssembly(fl);
			}
		}

		public void FindObjectsInAssembly(string assemblyFile)
		{
			Assembly assembly;
			if (!File.Exists(assemblyFile)) return;
			try {// пробуем загрузить
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e) {// если не загрузилось то показываем что к чему
				StateEngine.Log?.AddLog("Ошибка при загрузке сборки " + assemblyFile + Environment.NewLine + e.Message + Environment.NewLine + e.GetType());
				return; // и выходим
			}
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			var fname = assemblyFile.Substring(assemblyFile.LastIndexOf(@"\") + 1);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types) {
				if (type.FullName.Contains("<")) continue;
				if (type.FullName.Contains("+")) continue;
				if (type.FullName.Contains("`")) continue;
				var lvi = new ListViewItemFileClasses(fname, type);
				listView1.Items.Add(lvi);
				//Log(" " + type.FullName + " " + type.Name + " " + type.Namespace + " " + type.ToString());
			}
		}

	}
}