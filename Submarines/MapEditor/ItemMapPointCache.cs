using Submarines.Items;
using System.Collections.Generic;

namespace Submarines.MapEditor
{
    internal class ItemMapPointCache
    {
        private Dictionary<int, Vector> _cache = new Dictionary<int, Vector>();
        private List<ItemMapPoint> _values;
        public void InitCache(List<ItemMapPoint> values) {
            _values = values;
            Clear();
        }
        public void Clear() => _cache.Clear();

        public Vector GetValue(int num) {
            Vector res;
            if (_cache.TryGetValue(num, out res))
                return res;

            res = Vector.Zero();
            if (_values != null) {
                foreach (var item in _values) {
                    if (item.PointId != num)
                        continue;
                    res = item.Point;
                    _cache.Add(num, res);
                    break;
                }
            }
            return res;
        }
    }
}