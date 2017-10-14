using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Utils;
using DataSupport;
using DataSupportEF;
using System.Reflection;
using System.IO;
using Engine.Data;

namespace Utils_setup
{
	public partial class FormMain : Form
	{
		private LogSystem _ls;
		private DataSupportEF6 _DBContext;
		private void Log(string msg) { _ls.AddLog(msg); }

		public FormMain()
		{
			InitializeComponent();
			_ls = new LogSystem();
			_DBContext = new DataSupportEF6();
			_DBContext.InitLogSystem(_ls);
		}

		private void btnScan_Click(object sender, EventArgs e)
		{
			listView1.Items.Clear();
			var path = AppDomain.CurrentDomain.BaseDirectory;
			var files = Directory.GetFiles(path, "*.dll");
			foreach (var fl in files) {
				if (fl.Contains("EntityFramework.dll")) continue;
				if (fl.Contains("EntityFramework.SqlServer.dll")) continue;
				if (fl.Contains("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")) continue;
				if (fl.Contains("UnitTests.dll")) continue;
				FindObjectsInAssembly(fl);
			}
			var a = 1;
		}

		public void FindObjectsInAssembly(string assemblyFile)
		{
			Assembly assembly;
			if (!File.Exists(assemblyFile)) return;
			// пробуем загрузить
			try {
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e) {// если не загрузилось то показываем что к чему
				Log("Ошибка при загрузке сборки " + assemblyFile + Environment.NewLine + e.Message + Environment.NewLine + e.GetType());
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
				lvi.RefreshInfo();
				//Log(" " + type.FullName + " " + type.Name + " " + type.Namespace + " " + type.ToString());
			}
		}
	}
}
