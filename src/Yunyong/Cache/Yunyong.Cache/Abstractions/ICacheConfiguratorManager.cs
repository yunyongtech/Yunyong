using System;
using System.Collections.Generic;

namespace Yunyong.Cache.Abstractions
{
    /// <summary>
    ///     缓存配置管理
    /// </summary>
    public interface ICacheConfiguratorManager : IDisposable
    {
        /// <summary>
        ///     配置列表
        /// </summary>
        IReadOnlyList<ICacheConfigurator> CacheConfigurators { get; }

        /// <summary>
        ///     配置所有
        /// </summary>
        /// <param name="initAction"></param>
        void ConfigureAll(Action<ICache> initAction);

        /// <summary>
        ///     根据缓存名称配置
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="initAction"></param>
        void Configure(string cacheName, Action<ICache> initAction);
    }
}