using Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasViewer.Model.Entities
{
	class ModelAtlasTexture : EntityWithNotify
	{
		public AtlasTextures AtlasTextureData;
		public string AtlasTextureName {
			get { return AtlasTextureData.Name; }
			set { AtlasTextureData.Name = value; OnPropertyChanged(""); }
		}

		public string TextureDescriptionAtlasFile {
			get { return AtlasTextureData.Description; }
			set { AtlasTextureData.Description = value; OnPropertyChanged(""); }
		}

		public ModelAtlasTexture(AtlasTextures atlasTexture)
		{
			AtlasTextureData = atlasTexture;
		}
	}
}
