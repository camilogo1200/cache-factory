using System;
using System.Collections.Specialized;
using System.Configuration;
using Cache.Factory.Interfaces;
using Microsoft.ApplicationServer.Caching;

namespace Cache.Factory.CacheType
{
    public class AppFabricCache : ICacheBehavior
    {
        private DataCache _cache = null;
        private NameValueCollection config = null;

        public AppFabricCache()
        {
            config = (NameValueCollection)ConfigurationManager.GetSection("AppFabricParameters");
            CachingRuntimeCache _localCache = new CachingRuntimeCache();
            object AppCache = _localCache.GetItem("AppFabricCacheObject");
            if (AppCache != null)
                _cache = (DataCache)AppCache;
            else
            {
                DataCacheFactory factory = new DataCacheFactory();
                _cache = factory.GetCache(config["cacheName"]);
                _localCache.AddItem("AppFabricCacheObject", _cache);
            }
        }

        public Boolean AddItem(String key, Object value)
        {
            Boolean CacheFlag = false;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (ExistItem(key))
                        _cache.Remove(key);

                    DataCacheItemVersion version = _cache.Add(key, value, new TimeSpan(0, 20, 0));

                    if (ExistItem(key))
                        CacheFlag = true;
                    else
                        CacheFlag = false;
                }
                catch
                {
                }
            }
            return CacheFlag;
        }

        public Object GetItem(String key)
        {
            Object item = null;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    item = this._cache.Get(key);
                    if (item != null)
                        break;
                }
                catch
                {
                }
            }
            return item;
        }

        public Boolean DeleteItem(String key)
        {
            Boolean RemoveFlag = false;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    RemoveFlag = _cache.Remove(key);
                    if (RemoveFlag)
                        break;
                }
                catch
                {
                }
            }
            return RemoveFlag;
        }

        public Boolean ExistItem(String key)
        {
            if (GetItem(key) == null)
                return false;
            else
                return true;
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