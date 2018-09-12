using Yunyong.Cache.Abstractions;

namespace Yunyong.Cache.Redis
{
    public class RedisCacheManager : CacheManagerBase
    {
        private readonly IRedisCacheDatabaseProvider _redisCacheDatabaseProvider;

        public RedisCacheManager(IRedisCacheDatabaseProvider redisCacheDatabaseProvider,
            ICacheConfiguratorManager cacheConfiguratorManager) : base(cacheConfiguratorManager)
        {
            _redisCacheDatabaseProvider = redisCacheDatabaseProvider;
        }

        public override ICache CreateCacheImplementation(string cacheName)
        {
            return new RedisCache(cacheName, _redisCacheDatabaseProvider);
        }

        public override ICache CreateCacheImplementation(string cacheName, CacheDatabaseEnum cacheDatabaseType)
        {
            return new RedisCache(cacheName, _redisCacheDatabaseProvider, cacheDatabaseType);
        }
    }
}