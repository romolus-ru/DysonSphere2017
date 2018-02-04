using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizationOpenGL
{
	/// <summary>
	/// Описание текстуры из атласа
	/// </summary>
	public class Texture
	{
		public uint TextureCode;
		public int BlendParam;
		public int AtlasWidth;
		public int AtlasHeight;
		public int X;
		public int Y;
		public int Width;
		public int Height;
		public string AtlasName;
		public string TextureName;
		public string Description;
	}
}