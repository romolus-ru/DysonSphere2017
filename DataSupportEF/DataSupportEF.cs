using DataSupport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupportEF
{
	public class DataSupportEF : DataSupport.DataSupport
	{
		private DysonSphereContext ds;
		public DataSupportEF()
		{
			ds = new DysonSphereContext();
		}

		public override List<AtlasFiles> AtlasFilesGetAll()
		{
			return ds.AtlasFiles.ToList();
		}

		public override void SetLog(Action<string> log1)
		{
			ds.Database.Log += log1;
		}

	}
}
