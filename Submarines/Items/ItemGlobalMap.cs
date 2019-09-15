using System.Collections.Generic;
using System.Linq;

namespace Submarines.Items
{
    /// <summary>
    /// Глобальная карта - точки-карты и связь между ними
    /// </summary>
    internal class ItemGlobalMap
    {
        /// <summary>
        /// Точки-карты
        /// </summary>
        public List<ItemMapPoint> MapPoints { get; set; } = new List<ItemMapPoint>();
        public List<ItemMapRelation> MapRelations { get; set; } = new List<ItemMapRelation>();

        public int GetNewId() {
            var max = 0;
            foreach (var point in MapPoints) {
                if (max < point.PointId)
                    max = point.PointId;
            }
            return max + 1;
        }

        public ItemMapPoint GetPointById(int mapId) {
            return MapPoints.Where(mp => mp.PointId == mapId).FirstOrDefault();
        }
    }
}