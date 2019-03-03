using System.Collections.Generic;
using Submarines.Geometry;

namespace Submarines.Items
{
	/// <summary>
	/// Содержит характеристики карты и параметры в общем виде
	/// </summary>
	internal class ItemMap : ItemBase
	{
		public string MapCode { get; private set; }
		public string MapName { get; private set; }
		public string MapDescription { get; private set; }
		public GeometryBase MapGeometry { get; private set; }
		public List<ItemSpawnGroup> Spawns { get; private set; }
	}
}