using System;
using Cache.Factory.CacheType;
using Cache.Factory.Interfaces;

namespace Cache.Factory
{
    public sealed class FactoryCacheHelper : ICacheFactory
    {
        #region Singleton

        private static volatile FactoryCacheHelper instance;
        private static object syncRoot = new Object();

        private FactoryCacheHelper()
        {
        }

        public static FactoryCacheHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new FactoryCacheHelper();
                    }
                }
                return instance;
            }
        }

        #endregion Singleton

        private ICacheBehavior _runtimeCache = null;

        public ICacheBehavior RuntimeCache
        {
            get
            {
                if (_runtimeCache == null)
                {
                    lock (syncRoot)
                    {
                        if (_runtimeCache == null)
                        {
                            _runtimeCache = GetCacheType(ECacheType.CachingRuntimeCache);
                        }
                    }
                }
                return _runtimeCache;
            }
        }

        [Obsolete("Este metodo debe dejar de utilizarse, este metodo promueve multiples caches lo que puede llegar a ser un problema con la consistencia de la informacion, utilize el singleton de la clase")]
        public ICacheBehavior GetCache(ECacheType CacheType)
        {
            ICacheBehavior Resp = null;
            switch (CacheType)
            {
                case ECacheType.AppFabricCache:
                    Resp = new AppFabricCache();
                    break;

                case ECacheType.CacheBD:
                    Resp = new DBCache();
                    break;

                case ECacheType.CachingRuntimeCache:
                    Resp = new CachingRuntimeCache();
                    break;

                case ECacheType.EnterpriseLibraryCache:
                    Resp = new EnterpriseLibraryCache();
                    break;

                default:
                    throw new Exception("Invalid cache type ");
            }
            return Resp;
        }

        [Obsolete("Este metodo debe dejar de utilizarse, este metodo promueve multiples caches lo que puede llegar a ser un problema con la consistencia de la informacion, utilize el singleton de la clase")]
        public static ICacheBehavior GetCacheType(ECacheType CacheType)
        {
            ICacheBehavior Resp = null;
            switch (CacheType)
            {
                case ECacheType.AppFabricCache:
                    Resp = new AppFabricCache();
                    break;

                case ECacheType.CacheBD:
                    Resp = new DBCache();
                    break;

                case ECacheType.CachingRuntimeCache:
                    Resp = new CachingRuntimeCache();
                    break;

                case ECacheType.EnterpriseLibraryCache:
                    Resp = new EnterpriseLibraryCache();
                    break;

                default:
                    throw new Exception("Invalid cache type ");
            }
            return Resp;
        }

        public ICacheBehavior GetRuntimeCache()
        {
            return RuntimeCache;
        }
    }
}