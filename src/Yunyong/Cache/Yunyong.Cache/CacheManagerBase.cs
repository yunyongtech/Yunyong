using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Yunyong.Cache.Abstractions;

namespace Yunyong.Cache
{
    /// <summary>
    ///     缓存管理
    /// </summary>
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly ICacheConfiguratorManager CacheConfiguratorManager;
        protected readonly ConcurrentDictionary<string, ICache> Caches;

        protected CacheManagerBase(ICacheConfiguratorManager cacheConfiguratorManager)
        {
            CacheConfiguratorManager = cacheConfiguratorManager;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        /// <summary>
        ///     根据名称获取缓存
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public ICache GetCache(string cacheName)
        {
            return Caches.GetOrAdd(cacheName, x =>
            {
                var cache = CreateCacheImplementation(x);

                SetCacheExpireTimeConfig(cache);

                return cache;
            });
        }

        public ICache GetCache(string cacheName, CacheDatabaseEnum cacheDatabaseType)
        {
            return Caches.GetOrAdd(GetCacheName(cacheName, cacheDatabaseType), x =>
            {
                var cache = CreateCacheImplementation(cacheName, cacheDatabaseType);

                SetCacheExpireTimeConfig(cache);

                return cache;
            });
        }

        /// <summary>
        ///     获取所有缓存
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ICache> GetCaches()
        {
            return Caches.Values.ToImmutableList();
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public abstract ICache CreateCacheImplementation(string cacheName);

        /// <summary>
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="cacheDatabaseType"></param>
        /// <returns></returns>
        public abstract ICache CreateCacheImplementation(string cacheName, CacheDatabaseEnum cacheDatabaseType);

        /// <summary>
        ///     设置缓存过期配置
        /// </summary>
        /// <param name="cache"></param>
        private void SetCacheExpireTimeConfig(ICache cache)
        {
            var configurators = CacheConfiguratorManager
                .CacheConfigurators
                .Where(c => c.CacheName == null || c.CacheName == cache.CacheName);

            foreach (var configurator in configurators)
            {
                configurator.InitAction?.Invoke(cache);
            }
        }

        /// <summary>
        ///     获取缓存名称
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="cacheDatabaseType"></param>
        /// <returns></returns>
        private string GetCacheName(string cacheName, CacheDatabaseEnum cacheDatabaseType)
        {
            return $"db:{cacheDatabaseType}:{cacheName}";
        }
    }
}