using System.Collections.Generic;

namespace Engine.Utils
{
    /// <summary>
    /// Простой кэш, требует обновления извне
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class SimpleCache<T, U> {

        private Dictionary<T, U> _cache = new Dictionary<T, U>();

        public void Clear() => _cache.Clear();

        public void Add(T key, U value) {
            _cache[key] = value;
        }

        public U GetValue(T key, U defValue = default(U)) {
            U result;
            return _cache.TryGetValue(key, out result)
                ? result
                : defValue;
        }

    }
}