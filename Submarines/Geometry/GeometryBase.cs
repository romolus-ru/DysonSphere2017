using System.Collections.Generic;
using System.Drawing;

namespace Submarines.Geometry
{
	internal class GeometryBase
	{
		public string Name { get; set; }
		public Color Color { get; set; }
		public GeometryType GeometryType { get;set; }
		public List<LineInfo> Lines { get; set; } = null;
	}
}