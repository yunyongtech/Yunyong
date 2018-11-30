using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using Yunyong.Redis.Base;
using Yunyong.Redis.Interfaces;

namespace Yunyong.Redis
{
    public class SortedSetCache
        : Base.Cache, ISortedSetCache
    {
        public SortedSetCache(double ValidTimeSpan)
            : base(ValidTimeSpan)
        {
        }

        public long Length(string key)
        {
            return _DB.SortedSetLength(key, 0, int.MaxValue);
        }

        public bool Insert<T>(string key, T value, int sortFlag)
            where T : CacheValueBase
        {
            //
            if (string.IsNullOrWhiteSpace(key)
                || value == null)
            {
                return false;
            }

            //
            value.Key = key;
            var flag = _DB.SortedSetAdd(key, JsonConvert.SerializeObject(value), sortFlag);
            if (flag)
            {
                flag = Expire(_defaultValidTimeSpan, key);
            }
            return flag;
        }

        public bool Delete(params string[] keys)
        {
            return Delete(RedisType.SortedSet, keys);
        }

        public bool Delete<T>(string key, T value)
            where T : CacheValueBase
        {
            //
            value.Key = key;
            return _DB.SortedSetRemove(key, JsonConvert.SerializeObject(value));
        }

        public List<T> Select<T>(string key, int sortFlag)
            where T : CacheValueBase
        {
            //
            if (!Exist(key))
            {
                return new List<T>();
            }
            if (Length(key) <= 0)
            {
                return new List<T>();
            }

            //
            return _DB
                .SortedSetRangeByScore(key, sortFlag, sortFlag, Exclude.None, Order.Ascending, 0, -1, CommandFlags.None)
                ?.Select(it => JsonConvert.DeserializeObject<T>(it))
                ?.ToList();
        }

        public List<T> SelectPaged<T>(string key, int pageSize, int skipPage, string filterStr)
            where T : CacheValueBase
        {
            //
            if (!Exist(key))
            {
                return new List<T>();
            }
            if (Length(key) <= 0)
            {
                return new List<T>();
            }

            //
            return _DB
                .SortedSetScan(key, filterStr, pageSize, 0, skipPage)
                ?.Select(it => JsonConvert.DeserializeObject<T>(it.Element))
                ?.ToList();
        }

    }
}
