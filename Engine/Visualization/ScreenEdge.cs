using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Visualization
{
	/// <summary>
	/// Звено связывающее две точки
	/// </summary>
	public class ScreenEdge
	{
		public ScreenPoint A;
		public ScreenPoint B;
		public float Weight;

		public ScreenEdge(ScreenPoint a, ScreenPoint b)
		{
			A = a;
			B = b;
			Weight = A.distanceTo(B);
		}
		public bool IsEqual(ScreenEdge otherEdge)
		{
			if (this.A.IsEqual(otherEdge.A)
				&& this.B.IsEqual(otherEdge.B))
				return true;
			if (this.B.IsEqual(otherEdge.A)
				&& this.A.IsEqual(otherEdge.B))
				return true;
			return false;
		}
	}
}
