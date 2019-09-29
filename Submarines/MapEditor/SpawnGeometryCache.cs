using Submarines.Geometry;
using Submarines.Items;
using System.Collections.Generic;

namespace Submarines.MapEditor
{
    internal class SpawnGeometryCache
    {
        private Dictionary<string, GeometryBase> _cache = new Dictionary<string, GeometryBase>();

        public void InitCache() {
            Clear();
        }
        public void Clear() => _cache.Clear();

        public GeometryBase GetValue(ItemMap.ItemMapSpawnPoint spawn) {
            var areaName = spawn.AreaGeometryName;
            GeometryBase res = null;
            if (_cache.TryGetValue(areaName, out res))
                return res;

            res = ItemsManager.GetGeometry(areaName);
            _cache.Add(areaName, res);
            return res;
        }
    }
}