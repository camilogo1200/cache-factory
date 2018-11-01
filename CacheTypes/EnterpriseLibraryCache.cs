using System;
using Cache.Factory.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Cache.Factory.CacheType
{
    public class EnterpriseLibraryCache : ICacheBehavior
    {
        public Boolean AddItem(String key, Object value)
        {
            try
            {
                ICacheManager _cache = CacheFactory.GetCacheManager("CacheEnterpriseLibrary");
                _cache.Add(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Object GetItem(String key)
        {
            ICacheManager _cache = CacheFactory.GetCacheManager("CacheEnterpriseLibrary");
            return _cache.GetData(key);
        }

        public Boolean DeleteItem(String cacheKey)
        {
            try
            {
                ICacheManager _cache = CacheFactory.GetCacheManager("CacheEnterpriseLibrary");
                _cache.Remove(cacheKey);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean ExistItem(String cacheKey)
        {
            ICacheManager _cache = CacheFactory.GetCacheManager("CacheEnterpriseLibrary");
            if (_cache.Contains(cacheKey))
                return true;
            else
                return false;
        }

        public bool AddItem(string key, object value, int expireMinutes)
        {
            throw new NotImplementedException();
        }

        public bool CleanCache()
        {
            throw new NotImplementedException();
        }
    }
}