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
			// получаем классы из файлов
			foreach (var fl in files) {
				if (fl.Contains("EntityFramework.dll")) continue;
				if (fl.Contains("EntityFramework.SqlServer.dll")) continue;
				if (fl.Contains("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")) continue;
				if (fl.Contains("UnitTests.dll")) continue;
				FindObjectsInAssembly(fl);
			}
			var DBClasses = _DBContext.GetCollectClasses();
			foreach (var class1 in DBClasses) {
				// проверяем есть ли уже такой объект
				var founded = false;
				foreach (ListViewItemFileClasses item in listView1.Items) {
					if (item.FileName != class1.FileName) continue;
					if (item.ClassName != class1.ClassName) continue;
					// совпадает
					founded = true;
					item.InitWithDataFromDB(class1);
				}
				if (!founded) {// не нашли
					var lvi = new ListViewItemFileClasses(class1);
					listView1.Items.Add(lvi);
				}
			}
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
				//Log(" " + type.FullName + " " + type.Name + " " + type.Namespace + " " + type.ToString());
			}
		}

		private void btnAddChecked_Click(object sender, EventArgs e)
		{
			var list = listView1.CheckedItems;
			foreach (var item in list) {
				var lvi = item as ListViewItemFileClasses;
				if (lvi == null) continue;
				var cl1 = new CollectClass();
				cl1.FileName = lvi.FileName;
				cl1.ClassName = lvi.ClassName;
				_DBContext.SaveCollectClasses(cl1);
				lvi.InitWithDataFromDB(cl1);
				lvi.Checked = false;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var list = listView1.CheckedItems;
			foreach (var item in list) {
				var lvi = item as ListViewItemFileClasses;
				if (lvi == null) continue;
				var cl1 = lvi.Collect1;
				_DBContext.DeleteCollectClasses(cl1);
				lvi.InitWithDataFromDB(null);
				lvi.Checked = false;
			}
		}
	}
}
