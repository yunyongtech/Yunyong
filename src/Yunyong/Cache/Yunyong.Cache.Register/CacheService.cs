using System;
using System.Collections.Generic;
using StackExchange.Redis;
using Yunyong.Cache.Abstractions;

//using Rainbow.Common.Enums;

namespace Yunyong.Cache.Register
{
    public class CacheService<TCacheContext> : ICacheService<TCacheContext>
    {
        public List<T> GetByPattern<T>(string pattern)
        {
            return Cache.GetByPattern<T>($"*{Cache.CacheName}*{pattern}*");
        }
        public bool KeyExists(string key)
        {
            return Cache.KeyExists(key);
        }
        public List<T> GetAll<T>()
        {
            return Cache.GetByPattern<T>($@"*{Cache.CacheName}*");
        }

        #region Member

        private ICache Cache { get; set; }
        private readonly ICacheManager _cacheManager;

        public CacheService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
            var type = typeof(TCacheContext);
            Cache = _cacheManager.GetCache($"{type.Namespace}.{type.Name}");
        }

        #endregion

        #region Service

        public void SetCacheDatabaseContext(CacheDatabaseContextType cacheDatabaseContextType)
        {
            var type = typeof(TCacheContext);
            Cache = _cacheManager.GetCache($"{type.Namespace}.{type.Name}",
                (CacheDatabaseEnum) cacheDatabaseContextType);
        }

        public T GetOrDefault<T>(string key)
        {
            return Cache.GetOrDefault<T>(key);
        }

        public T GetOrDefault<T>(string key, Func<T> func, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            var tmp = Cache.GetOrDefault<T>(key);
            if (tmp != null)
            {
                return tmp;
            }

            Set(key, func(), slidingExpireTime, absoluteExpireTime);
            tmp = Cache.GetOrDefault<T>(key);

            return tmp;
        }

        public object GetOrDefault(string key, Type type)
        {
            return Cache.GetOrDefault(key, type);
        }



        public void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            Cache.Set(key, value, slidingExpireTime, absoluteExpireTime);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public string[] GetKeys(string pattern = "")
        {
            var result = Cache.Execute("keys", $"*{Cache.CacheName}*{pattern}*") as RedisResult;
            return (string[]) result;
        }

        public object GetStringKeyValues(string pattern = "")
        {
            return Cache.GetKeyValues($"*{Cache.CacheName}*{pattern}*");
        }



        #endregion
    }
}