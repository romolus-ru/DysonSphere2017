﻿// insert or update entity
// https://msdn.microsoft.com/en-us/library/jj592676(v=vs.113).aspx
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Data;

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

		public override void SaveChanges()
		{
			base.SaveChanges();
			ds.SaveChanges();
		}

		public override void SetLog(Action<string> log1)
		{
			ds.Database.Log += log1;
		}

		public override List<AtlasFiles> AtlasFilesGetAll()
		{
			var a = ds.AtlasFiles.ToList();
			Log("atlasFilesGet count="+a.Count);
			return a;
		}

		public override AtlasFiles GetAtlasFile(string atlasName)
		{
			return ds.AtlasFiles.Where(obj => obj.AtlasName == atlasName).ToList().FirstOrDefault();
		}

		public override List<AtlasTextures> GetAtlasTextures(long atlasId)
		{
			var list = ds.AtlasTextures.Where(obj => obj.AtlasFileId == atlasId).ToList();
			return list;
		}

		public override void AddAtlasFile(AtlasFiles atlasFile)
		{
			ds.Entry(atlasFile).State = atlasFile.IdAtlasFile == 0 ?
					   EntityState.Added :
					   EntityState.Modified;
			ds.SaveChanges();
		}

		public override void DeleteAtlasFile(AtlasFiles atlasFile)
		{
			ds.Entry(atlasFile).State = EntityState.Deleted;
			ds.SaveChanges();
		}

		public override void AddAtlasTexture(AtlasTextures atlasTexture)
		{
			ds.Entry(atlasTexture).State = atlasTexture.IdAtlasLink == 0 ?
					   EntityState.Added :
					   EntityState.Modified;
			ds.SaveChanges();
		}

		public override void DeleteAtlasTexture(AtlasTextures atlasTexture)
		{
			ds.Entry(atlasTexture).State = EntityState.Deleted;
			ds.SaveChanges();
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

		public override void DeleteCollectClasses(CollectClass collectClass, bool save = true)
		{
			ds.Entry(collectClass).State = EntityState.Deleted;
			if (save) ds.SaveChanges();
		}

		private Dictionary<string, int> _settings = null;

		private void ServerSettingsGetAll()
		{
			_settings = new Dictionary<string, int>();
			var sts = ds.Settings.Where(s => s.TargetSys == "Server").ToList();
			foreach (var item in sts) {
				if (!_settings.ContainsKey(item.TargetSubSys)) {
					_settings.Add(item.TargetSubSys, item.ClassId);
				}
			}
		}

		public override int ServerSettingsGetValue(string valueName)
		{
			if (_settings == null) {
				ServerSettingsGetAll();
			}
			if (!_settings.ContainsKey(valueName)) {
				Log("значение " + valueName + " не обнаружено в настройках");
				return -1;
			}
			return _settings[valueName];
		}

		public override void ServerSettingsSetValue(string valueName, int classId)
		{
			var st1 = ds.Settings.Where(s => s.TargetSys == "Server" && s.TargetSubSys == "valueName").FirstOrDefault();
			if (st1 == null) st1 = new _Settings();
			st1.TargetSys = "Server";
			st1.TargetSubSys = valueName;
			st1.ClassId = classId;
			ds.Entry(st1).State = st1.IdSettings == 0 ?
					   EntityState.Added :
					   EntityState.Modified;
			ds.SaveChanges();
			ServerSettingsGetAll();
		}

	}
}