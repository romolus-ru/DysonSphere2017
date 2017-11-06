using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationProviderOpenGL
{
	/// <summary>
	/// Описание текстуры из атласа
	/// </summary>
	public class Texture
	{
		public uint TextureCode;
		public int AtlasWidth;
		public int AtlasHeight;
		public int P1X;
		public int P1Y;
		public int P2X;
		public int P2Y;
	}
}