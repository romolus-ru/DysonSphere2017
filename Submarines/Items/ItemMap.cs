using System.Collections.Generic;

namespace Submarines.Items
{
    /// <summary>
    /// Содержит характеристики карты и параметры в общем виде
    /// </summary>
    internal class ItemMap
    {
        /// <summary>
        /// точка спавна - координата и имя
        /// </summary>
        internal class ItemMapSpawnPoint
        {
            public string Name;
            public Vector Point;
        }
		public string MapCode { get; set; }
		public string MapName { get; set; }
		public string MapDescription { get; set; }
		public string MapGeometryName { get; set; }
		public List<ItemMapSpawnPoint> MapSpawns { get; set; }
	}
}