using Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasViewer.Model.Entities
{
	public class ModelAtlasTexture : EntityWithNotify
	{
		public AtlasTextures AtlasTextureData;
		public string AtlasTextureName {
			get { return AtlasTextureData.Name; }
			set { AtlasTextureData.Name = value; OnPropertyChanged(""); OnPropertyChanged("NameDescription"); }
		}

		public string TextureDescriptionAtlasFile {
			get { return AtlasTextureData.Description; }
			set { AtlasTextureData.Description = value; OnPropertyChanged(""); OnPropertyChanged("NameDescription"); }
		}

		public long P1X {
			get { return AtlasTextureData.P1X; }
			set { AtlasTextureData.P1X = value; OnPropertyChanged(""); OnPropertyChanged("Size"); }
		}

		public long P1Y {
			get { return AtlasTextureData.P1Y; }
			set { AtlasTextureData.P1Y = value; OnPropertyChanged(""); OnPropertyChanged("Size"); }
		}

		public long P2X {
			get { return AtlasTextureData.P2X; }
			set { AtlasTextureData.P2X = value; OnPropertyChanged(""); OnPropertyChanged("Size"); }
		}

		public long P2Y {
			get { return AtlasTextureData.P2Y; }
			set { AtlasTextureData.P2Y = value; OnPropertyChanged(""); OnPropertyChanged("Size"); }
		}

		public string NameDescription {
			get { return AtlasTextureData.Name + "." + AtlasTextureData.Description; }
		}

		public string Size {
			get {
				return AtlasTextureData.P1X + "," + AtlasTextureData.P1Y + " " + AtlasTextureData.P2X + "," + AtlasTextureData.P2Y
				  + " " + (AtlasTextureData.P2X - AtlasTextureData.P1X) + "x" + (AtlasTextureData.P2Y - AtlasTextureData.P1Y); }
		}

		public ModelAtlasTexture(AtlasTextures atlasTexture)
		{
			AtlasTextureData = atlasTexture;
		}

	}
}
