using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game.Resource
{
	/// <summary>
	/// Значение ресурса
	/// </summary>
	public class ResourceValue
	{
		public ResourcesEnum Res;
		public int Value;
		public ResourceValue GetCopy()
		{
			var r = new ResourceValue();
			r.Res = Res;
			r.Value = Value;
			return r;
		}
	}
}