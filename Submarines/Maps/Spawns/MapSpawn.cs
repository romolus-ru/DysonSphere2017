using Submarines.Geometry;
using Submarines.Items;

namespace Submarines.Maps.Spawns
{
    /// <summary>
    /// Точка на карте, выполняющая конкретное действие. например телепорт, вход в город, активация триггера, создание противника и т.п.
    /// </summary>
    internal class MapSpawn
    {
        public int Id;
        public GeometryBase Geometry;
        public SpawnType SpawnType;
        public Vector Point;
        
        /// <summary>
        /// Для блокировки точки в момент старта карты
        /// </summary>
        public bool ActiveCollision;
    }
}