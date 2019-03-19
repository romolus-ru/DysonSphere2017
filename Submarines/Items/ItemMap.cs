using System.Collections.Generic;

namespace Submarines.Items
{
	/// <summary>
	/// Содержит характеристики карты и параметры в общем виде
	/// </summary>
	internal class ItemMap
	{
		public string MapCode { get; set; }
		public string MapName { get; set; }
		public string MapDescription { get; set; }
		public string MapGeometryName { get; set; }
		public List<string> MapSpawnsNames { get; set; }
	}
}