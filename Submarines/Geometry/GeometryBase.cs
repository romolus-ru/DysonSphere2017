using System.Collections.Generic;
using System.Drawing;

namespace Submarines.Geometry
{
	internal class GeometryBase
	{
		public string Name { get; private set; }
		public Color Color { get; private set; }
		public GeometryType GeometryType { get;private set; }
		public List<KeyValuePair<Vector, Vector>> Lines = null;
	}
}