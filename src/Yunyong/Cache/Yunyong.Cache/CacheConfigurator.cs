using System;
using Yunyong.Cache.Abstractions;

namespace Yunyong.Cache
{
    public class CacheConfigurator : ICacheConfigurator
    {
        /// <summary>
        ///     配置
        /// </summary>
        /// <param name="initAction"></param>
        public CacheConfigurator(Action<ICache> initAction)
        {
            InitAction = initAction;
        }

        /// <summary>
        ///     根据缓存名称配置
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="initAction"></param>
        public CacheConfigurator(string cacheName, Action<ICache> initAction)
        {
            CacheName = cacheName;
            InitAction = initAction;
        }

        public string CacheName { get; }

        public Action<ICache> InitAction { get; }

        public virtual void Dispose()
        {
        }
    }
}