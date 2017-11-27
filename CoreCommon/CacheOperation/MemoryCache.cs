using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CoreCommon.CacheOperation
{
    public class MemoryCache<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, TValue> _dicCache = new ConcurrentDictionary<TKey, TValue>();
        private Dictionary<TKey, Lazy<TValue>> _dicLazyValue = new Dictionary<TKey, Lazy<TValue>>();

        private object _locker = new object();

        public TValue GetValue(TKey key, Lazy<TValue> value)
        {
            lock (_locker)
            {
                if (!_dicLazyValue.ContainsKey(key))
                {
                    _dicLazyValue.Add(key, value);
                }

                if (_dicCache == null)
                {
                    _dicCache = new ConcurrentDictionary<TKey, TValue>();
                }
            }
            return _dicCache.GetOrAdd(key, _dicLazyValue[key].Value);
        }
    }
}
