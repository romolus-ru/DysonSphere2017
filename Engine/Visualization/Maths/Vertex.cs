using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
  copyright s-hull.org 2011
  released under the contributors beerware license
  contributors: Phil Atkin, Dr Sinclair.
*/
namespace Engine.Visualization.Maths
{
	public class Vertex
	{
		public float x, y;
		public ScreenPoint LinkPoint = null;

		protected Vertex() { }

		public Vertex(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public Vertex(ScreenPoint linkPoint) : this(linkPoint.X, linkPoint.Y)
		{
			LinkPoint = linkPoint;
		}

		public float distance2To(Vertex other)
		{
			float dx = x - other.x;
			float dy = y - other.y;
			return dx * dx + dy * dy;
		}

		public float distanceTo(Vertex other)
		{
			return (float)Math.Sqrt(distance2To(other));
		}

		public override string ToString()
		{
			return string.Format("({0},{1})", x, y);
		}
	}

}
