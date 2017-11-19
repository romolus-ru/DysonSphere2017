using Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasViewer.Model.Entities
{
	public class ModelAtlasFile : EntityWithNotify
	{
		public AtlasFiles AtlasFileData;
		public string AtlasName {
			get { return AtlasFileData.AtlasName; }
			set { AtlasFileData.AtlasName = value; OnPropertyChanged(""); }
		}

		public string AtlasFile {
			get { return AtlasFileData.AtlasFile; }
			set { AtlasFileData.AtlasFile = value; OnPropertyChanged(""); }
		}

		public ModelAtlasFile(AtlasFiles atlasFile)
		{
			AtlasFileData = atlasFile;
		}
	}
}
