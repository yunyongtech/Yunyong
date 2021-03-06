﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Yunyong.Cache.Redis
{
    public class RedisCache : CacheBase
    {
        private readonly string _cacheName;
        private readonly IDatabase _database;

        public RedisCache(string cacheNane, IRedisCacheDatabaseProvider redisCacheDatabaseProvider) : base(cacheNane)
        {
            _database = redisCacheDatabaseProvider.GetDatabase();
            _cacheName = cacheNane;
        }

        public RedisCache(string cacheNane, IRedisCacheDatabaseProvider redisCacheDatabaseProvider,
            CacheDatabaseEnum cacheDatabaseType) : base(cacheNane, cacheDatabaseType)
        {
            _database = redisCacheDatabaseProvider.GetDatabase(cacheDatabaseType);
            _cacheName = cacheNane;
        }

        public override bool KeyExists(string key)
        {
            return _database.KeyExists(GetKey(key));
        }

        public override T GetOrDefault<T>(string key)
        {
            var newKey = GetKey(key);

            var redisValue = _database.StringGet(newKey);

            return redisValue.HasValue ? JsonConvert.DeserializeObject<T>(redisValue) : default(T);
        }

        public override object GetOrDefault(string key, Type type)
        {
            var newKey = GetKey(key);

            var redisValue = _database.StringGet(newKey);

            return redisValue.HasValue ? JsonConvert.DeserializeObject(redisValue, type) : default;
        }

        public override string GetJson(string key)
        {
            var newKey = GetKey(key);

            var redisValue = _database.StringGet(newKey);

            return redisValue.HasValue ? redisValue.ToString() : default(string);
        }

        public override List<T> GetByPattern<T>(string pattern)
        {
            //Cache.Execute("keys", $"*{Cache.CacheName}*{pattern}*") as RedisResult;
            var keys = _database.Execute("keys", pattern);
            var vals = _database.StringGet((RedisKey[])keys);
            var result = new List<T>();
            foreach (var redisValue in vals)
            {
                try
                {
                    if (redisValue.HasValue)
                    {
                        result.Add(JsonConvert.DeserializeObject<T>(redisValue));
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }

        /// <summary>
        ///     设置缓存，绝对过期时间高于相对过期时间
        /// </summary>
        /// <param name="key">键名称</param>
        /// <param name="value">值</param>
        /// <param name="slidingExpireTime">相对过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null,
            TimeSpan? absoluteExpireTime = null)
        {
            key = GetKey(key);
            var json = JsonConvert.SerializeObject(value);
            if (slidingExpireTime.HasValue || absoluteExpireTime.HasValue)
            {
                if (absoluteExpireTime.HasValue && absoluteExpireTime.Value != TimeSpan.Zero)
                {
                    _database.StringSet(key, json, absoluteExpireTime);
                }
                else if (slidingExpireTime.HasValue && slidingExpireTime.Value != TimeSpan.Zero)
                {
                    _database.StringSet(key, json, slidingExpireTime, When.Always, CommandFlags.FireAndForget);
                }
                else
                {
                    _database.StringSet(key, JsonConvert.SerializeObject(value));
                }
            }
            else
            {
                //永不过期
                _database.StringSet(key, JsonConvert.SerializeObject(value));
            }
        }

        public override void Remove(string key)
        {
            key = GetKey(key);
            _database.KeyDelete(key);
        }

        public override object Execute(string command, params object[] args)
        {
            return _database.Execute(command, args);
        }

        public override object GetKeyValues(string pattern)
        {
            var result = _database.Execute("keys", pattern);
            return _database.StringGet((RedisKey[])result);
        }


        private string GetKey(string key)
        {
            return "n:" + _cacheName + ",c:" + key;
        }


    }
}