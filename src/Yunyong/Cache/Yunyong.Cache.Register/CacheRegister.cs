using Microsoft.Extensions.DependencyInjection;
using Yunyong.Cache.Abstractions;
using Yunyong.Cache.Redis;

//using Rainbow.Common.Configs;

namespace Yunyong.Cache.Register
{
    /// <summary>
    ///     缓存注入
    /// </summary>
    public static class CacheRegister
    {
        public static IServiceCollection RegisterRedisCache(this IServiceCollection serviceCollection,
            CacheServiceConfig cacheServiceConfig)
        {
            //缓存数据库配置
            var redisCacheDatabaseProviderConfig = new RedisCacheDatabaseProviderConfig
            {
                ConnectionString = $"{cacheServiceConfig.ConnectionString.Trim('"')}:{cacheServiceConfig.Port}"
                //ConnectionString = "192.168.0.20:6379"
            };
            if (!string.IsNullOrEmpty(cacheServiceConfig.Password))
            {
                redisCacheDatabaseProviderConfig.ConnectionString =
                    $"{redisCacheDatabaseProviderConfig.ConnectionString},password={cacheServiceConfig.Password.Trim('"')}";
            }

            serviceCollection.AddSingleton(redisCacheDatabaseProviderConfig);
            //缓存过期配置
            serviceCollection.AddSingleton<ICacheConfiguratorManager>(provider =>
            {
                //默认过期时间1个小时
                var config = new CacheConfiguratorManager();
                //设置所有过期时间为20分钟
                //config.ConfigureAll(cache => { cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(20); });
                //设置缓存组过期时间
                //config.Configure("CacheName", cache => { cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(20); });

                return config;
            });
            //数据库注入
            serviceCollection.AddSingleton(typeof(IRedisCacheDatabaseProvider), typeof(RedisCacheDatabaseProvider));
            //redis注入
            serviceCollection.AddSingleton(typeof(ICacheManager), typeof(RedisCacheManager));
            //缓存注入
            serviceCollection.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));

            return serviceCollection;
        }
    }
}