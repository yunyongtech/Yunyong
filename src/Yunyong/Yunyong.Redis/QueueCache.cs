using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using Yunyong.Redis.Base;
using Yunyong.Redis.Interfaces;

namespace Yunyong.Redis
{
    public class QueueCache 
        : ListCache, IQueueCache
    {
        public QueueCache(double ValidTimeSpan)
            : base(ValidTimeSpan)
        { }

        public bool Push<T>(string key, T value)
            where T : CacheValueBase
        {
            return PushBatch(key, new List<T> { value });
        }

        public bool PushBatch<T>(string key, List<T> values)
            where T : CacheValueBase
        {
            //
            if (string.IsNullOrWhiteSpace(key)
                || values == null
                || values.Count <= 0)
            {
                return false;
            }

            //
            var vals = new List<RedisValue>();
            values.ForEach(it =>
            {
                it.Key = key;
                vals.Add(JsonConvert.SerializeObject(it));
            });
            var flag = _DB.ListLeftPush(key, vals.ToArray()) > 0;
            if (flag)
            {
                flag = Expire(_defaultValidTimeSpan, key);
            }
            return flag;
        }

        public T Pop<T>(string key)
            where T : CacheValueBase
        {
            //
            if (!Exist(key))
            {
                return null;
            }
            if (Length(key) <= 0)
            {
                return null;
            }

            //
            return JsonConvert.DeserializeObject<T>(_DB.ListRightPop(key));
        }

    }
}
