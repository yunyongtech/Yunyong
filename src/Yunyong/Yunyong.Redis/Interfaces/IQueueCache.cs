using System;
using System.Collections.Generic;
using System.Text;
using Yunyong.Redis.Base;

namespace Yunyong.Redis.Interfaces
{
    public interface IQueueCache
        :IListCache
    {
        bool Push<T>(string key, T value)
            where T : CacheValueBase;
        bool PushBatch<T>(string key, List<T> values)
            where T : CacheValueBase;

        T Pop<T>(string key)
           where T : CacheValueBase;
    }
}
