using System;
using System.Collections.Generic;
using Yunyong.Cache.Abstractions;

namespace Yunyong.Cache
{
    /// <summary>
    ///     缓存服务
    /// </summary>
    public abstract class CacheBase : ICache
    {
        #region Member

        /// <summary>
        ///     使用默认库
        /// </summary>
        /// <param name="cacheName"></param>
        protected CacheBase(string cacheName)
        {
            CacheName = cacheName;
            CacheDatabaseEnum = CacheDatabaseEnum.Default;
        }

        /// <summary>
        ///     使用指定库
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="cacheDatabaseType"></param>
        protected CacheBase(string cacheName, CacheDatabaseEnum cacheDatabaseType)
        {
            CacheName = cacheName;
            CacheDatabaseEnum = cacheDatabaseType;
        }

        public string CacheName { get; }
        public CacheDatabaseEnum CacheDatabaseEnum { get; }

        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        #endregion

        #region Service

        public abstract T GetOrDefault<T>(string key);


        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null);


        public abstract void Remove(string key);

        public abstract object Execute(string command, params object[] args);
        public abstract object GetKeyValues(string pattern);
        public abstract List<T> GetByPattern<T>(string pattern);

        public virtual void Dispose()
        {
        }

        #endregion
    }
}