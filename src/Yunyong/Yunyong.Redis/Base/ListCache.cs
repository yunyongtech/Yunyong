using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yunyong.Redis.Base
{
    public abstract class ListCache : Cache
    {
        internal ListCache(double ValidTimeSpan)
            : base(ValidTimeSpan)
        { }

        public bool Delete(params string[] keys)
        {
            return Delete(RedisType.List, keys);
        }

        public int Length(string key)
        {
            return (int)_DB.ListLength(key);
        }

    }
}
