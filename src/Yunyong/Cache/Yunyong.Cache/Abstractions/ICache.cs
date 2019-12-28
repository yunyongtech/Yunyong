using System;
using System.Collections.Generic;

namespace Yunyong.Cache.Abstractions
{
    /// <summary>
    ///     缓存服务
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        ///     缓存名称
        /// </summary>
        string CacheName { get; }

        /// <summary>
        ///     缓存数据库上下文
        /// </summary>
        CacheDatabaseEnum CacheDatabaseEnum { get; }

        /// <summary>
        ///     默认相对过期时间
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }

        /// <summary>
        ///     默认绝对过期时间
        /// </summary>
        TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        bool KeyExists(string key);

        /// <summary>
        ///     获取key值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetOrDefault<T>(string key);

        /// <summary>
        /// 获取Key值Json
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetJson(string key);
        /// <summary>
        ///     获取key值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Object GetOrDefault(string key, Type type);

        /// <summary>
        ///     设置key值
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        ///     执行命令
        /// </summary>
        /// <param name="command">命令名称</param>
        /// <param name="args">命令参数</param>
        /// <returns></returns>
        object Execute(string command, params object[] args);

        /// <summary>
        ///     模糊查询获取KeyValue集合
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        object GetKeyValues(string pattern);

        List<T> GetByPattern<T>(string pattern);
    }
}