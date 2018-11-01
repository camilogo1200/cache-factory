using Cache.Factory.CacheType;

namespace Cache.Factory.Interfaces
{
    public interface ICacheFactory
    {
        ICacheBehavior GetCache(ECacheType CacheType);

        ICacheBehavior GetRuntimeCache();
    }
}