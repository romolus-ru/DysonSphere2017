using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Строение построенное на планете. без этого планета ничего не производит
	/// </summary>
	[DebuggerDisplay("{BuilingType}")]
	public class Building
	{
		public BuildingEnum BuilingType = BuildingEnum.Nope;
	}
}
