using System;
using System.Collections.Generic;

namespace Yunyong.Cache.Abstractions
{
    /// <summary>
    ///     缓存服务
    /// </summary>
    /// <typeparam name="TCacheContext"></typeparam>
    public interface ICacheService<TCacheContext>
    {
        /// <summary>
        ///     检查Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool KeyExists(string key);

        /// <summary>
        ///     获取key值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetOrDefault<T>(string key);
        /// <summary>
        ///     获取Key值，没有则设置默认值并获取
        /// </summary>
        T GetOrDefault<T>(string key, Func<T> func, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        ///     获取key值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object GetOrDefault(string key, Type type);
        /// <summary>
        ///     设置key值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        /// <param name="absoluteExpireTime"></param>
        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        ///     移除key值
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        ///     模糊查询获取Key集合
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        string[] GetKeys(string pattern = "");

        /// <summary>
        ///     模糊查询获取KeyValue集合
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        object GetStringKeyValues(string pattern = "");

        /// <summary>
        ///     显示设置缓存数据库上下文，否则使用配置默认库
        /// </summary>
        /// <param name="cacheDatabaseContextType"></param>
        void SetCacheDatabaseContext(CacheDatabaseContextType cacheDatabaseContextType);

        /// <summary>
        ///     Gets the by pattern.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        List<T> GetByPattern<T>(string pattern);

        /// <summary>
        ///     Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> GetAll<T>();
    }
}