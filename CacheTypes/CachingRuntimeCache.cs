using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Cache.Factory.Interfaces;

namespace Cache.Factory.CacheType
{
    public class CachingRuntimeCache : ICacheBehavior
    {
        public Boolean AddItem(String key, Object value)
        {
            ObjectCache _cache = MemoryCache.Default;
            return _cache.Add(key.ToUpper(), value, DateTime.Now.AddMinutes(120));
        }

        public Object GetItem(String key)
        {
            ObjectCache _cache = MemoryCache.Default;
            return (_cache.Get(key.ToUpper()));
        }

        public Boolean DeleteItem(String cachekey)
        {
            ObjectCache _cache = MemoryCache.Default;
            if (_cache.Remove(cachekey.ToUpper()) == null)
                return true;
            else
                return false;
        }

        public Boolean ExistItem(String cachekey)
        {
            ObjectCache _cache = MemoryCache.Default;
            return _cache.Contains(cachekey.ToUpper());
        }

        public bool AddItem(string key, object value, int expireMinutes)
        {
            ObjectCache _cache = MemoryCache.Default;
            return _cache.Add(key.ToUpper(), value, DateTime.Now.AddMinutes(expireMinutes));
        }

        public bool CleanCache()
        {
            List<string> cachekeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
            foreach (string cachekey in cachekeys)
                MemoryCache.Default.Remove(cachekey);
            return true;
        }
    }
}