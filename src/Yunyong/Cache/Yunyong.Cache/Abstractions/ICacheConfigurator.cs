using System;

namespace Yunyong.Cache.Abstractions
{
    /// <summary>
    ///     缓存配置项
    /// </summary>
    public interface ICacheConfigurator : IDisposable
    {
        /// <summary>
        ///     缓存名称
        /// </summary>
        string CacheName { get; }

        /// <summary>
        ///     配置
        /// </summary>
        Action<ICache> InitAction { get; }
    }
}