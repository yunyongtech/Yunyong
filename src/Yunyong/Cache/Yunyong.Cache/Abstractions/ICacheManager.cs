using System;
using System.Collections.Generic;

namespace Yunyong.Cache.Abstractions
{
    /// <summary>
    ///     缓存管理
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        ///     根据名称获取缓存
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        ICache GetCache(string cacheName);

        /// <summary>
        ///     根据名称获取缓存
        /// </summary>
        /// <param name="cacheName">缓存名称</param>
        /// <param name="cacheDatabaseType">缓存数据库类型</param>
        /// <returns></returns>
        ICache GetCache(string cacheName, CacheDatabaseEnum cacheDatabaseType);

        /// <summary>
        ///     获取所有缓存
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<ICache> GetCaches();
    }
}