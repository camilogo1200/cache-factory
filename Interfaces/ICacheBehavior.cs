using System;

namespace Cache.Factory.Interfaces
{
    public interface ICacheBehavior
    {
        Boolean AddItem(String key, Object value);

        Boolean AddItem(String key, Object value, int expireMinutes);

        Object GetItem(String key);

        Boolean DeleteItem(String cacheKey);

        Boolean ExistItem(String cacheKey);

        Boolean CleanCache();
    }
}