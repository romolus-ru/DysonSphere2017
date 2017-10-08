using DataSupport;
using DataSupport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	}
}