// insert or update entity
// https://msdn.microsoft.com/en-us/library/jj592676(v=vs.113).aspx
using DataSupport;
using DataSupport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Data;
using System.Data.Entity;

namespace DataSupportEF
{
	public class DataSupportEF6 : DataSupportBase
	{
		private DysonSphereContext ds;
		protected override string LogTag { get { return "DataSupportEF6"; } }
		public DataSupportEF6()
		{
			ds = new DysonSphereContext();
		}

		public override List<AtlasFiles> AtlasFilesGetAll()
		{
			var a = ds.AtlasFiles.ToList();
			Log("atlasFilesGet count="+a.Count);
			return a;
		}

		public override void SetLog(Action<string> log1)
		{
			ds.Database.Log += log1;
		}

		public override List<CollectClass> GetCollectClasses()
		{
			return ds.CollectClasses.ToList();
		}

		public override void SaveCollectClasses(CollectClass collectClass, bool save=true)
		{
			ds.Entry(collectClass).State= collectClass.Id == 0 ?
								   EntityState.Added :
								   EntityState.Modified;
			if (save) ds.SaveChanges();
		}

		private Dictionary<string, int> _settings = null;

		public override int ServerSettingsGetValue(string valueName)
		{
			if (_settings == null) {
				_settings = new Dictionary<string, int>();
				var sts = ds.Settings.ToList();
				foreach (var item in sts) {
					if (!_settings.ContainsKey(item.TargetSubSys)) {
						_settings.Add(item.TargetSubSys, item.ClassId);
					}
				}
			}
			if (!_settings.ContainsKey(valueName)) {
				Log("значение " + valueName + " не обнаружено в настройках");
				return -1;
			}
			return _settings[valueName];
		}

	}
}