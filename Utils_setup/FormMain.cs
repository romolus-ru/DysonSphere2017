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
using System.Reflection;
using System.IO;
using Engine;
using Engine.Data;
using Engine.Visualization;
using DataSupportEF;
using Newtonsoft.Json;
using Engine.Helpers;

namespace Utils_setup
{
	public partial class FormMain : Form
	{
		private LogSystem _ls;
		private DataSupportEF6 _DBContext;
		private string appPath = "";
		private void Log(string msg) { _ls.AddLog(msg); }

		public FormMain()
		{
			InitializeComponent();
			_ls = new LogSystem();
			_DBContext = new DataSupportEF6();
			_DBContext.InitLogSystem(_ls);
			appPath = AppDomain.CurrentDomain.BaseDirectory;
		}

		private void btnScan_Click(object sender, EventArgs e)
		{
			listView1.Items.Clear();
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
			try {// пробуем загрузить
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
				_DBContext.DeleteCollectClasses(cl1,false);
				lvi.InitWithDataFromDB(null);
				lvi.Checked = false;
			}
			_DBContext.SaveChanges();
		}

		private void SetObjectSettings(string settingsName,Type objectType)
		{
			var list = listView1.CheckedItems;
			if (list.Count < 1) return;
			var lvi = list[0] as ListViewItemFileClasses;
			if (lvi == null) return;
			var cl1 = lvi.Collect1;
			var assemblyFile = appPath + lvi.FileName;

			Assembly assembly;
			if (!File.Exists(assemblyFile)) return;
			try {// пробуем загрузить
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception ex) {// если не загрузилось то показываем что к чему
				Log("Ошибка при загрузке сборки " + assemblyFile + Environment.NewLine + ex.Message + Environment.NewLine + ex.GetType());
				return; // и выходим
			}

			// ищем нужные типы в объектах и сохраняем их для последующего использования
			var fname = assemblyFile.Substring(assemblyFile.LastIndexOf(@"\") + 1);
			Type[] types = assembly.GetTypes();
			//Type objectType = typeof(VisualizationProvider);

			var pE = from pe in types where pe != objectType select pe;
			foreach (Type type in pE) {
				var searchType = type;
				var founded = false; // флаг что нашли тип с нужным предком
				while (searchType != typeof(object)) {
					if (searchType == objectType) {
						founded = true; // нашли, выходим
						break;
					}
					// идём выше по иерархии
					if (searchType.BaseType != null) searchType = searchType.BaseType;
					// или прерываем поиск
					if (searchType.BaseType == null) break;
				}
				if (founded) {
					_DBContext.ServerSettingsSetValue(settingsName, cl1.Id);
					break;
				}
			}
			foreach (var item in list) {
				var lvi1 = item as ListViewItem;
				lvi1.Checked = false;
			}
		}

		private void btnSetVisualization_Click(object sender, EventArgs e)
		{
			SetObjectSettings("visualization", typeof(VisualizationProvider));
		}

		private void btnSetInput_Click(object sender, EventArgs e)
		{
			SetObjectSettings("input", typeof(Input));
		}

		private void btnDBtoJSON_Click(object sender, EventArgs e)
		{
			var dataDB = new DysonSphereContext();
			var values=dataDB.CollectClasses.ToList();
			var data = JsonConvert.SerializeObject(values);
			FileUtils.SaveString(DataSupportFileHelper.CollectClassesFile, DataSupportFileHelper.CollectClassesData, data);

			var settings = new Dictionary<string, int>();
			// пока добавляем серверные. потом эти настройки надо сделать полностью автономными
			var sts = dataDB.Settings.Where(s => s.TargetSys == "Server").ToList();
			foreach (var item in sts) {
				if (!settings.ContainsKey(item.TargetSubSys)) {
					settings.Add(item.TargetSubSys, item.ClassId);
				}
			}
			data = JsonConvert.SerializeObject(settings);
			FileUtils.SaveString(DataSupportFileHelper.SettingsFile, DataSupportFileHelper.SettingsData, data);
		}
	}
}
