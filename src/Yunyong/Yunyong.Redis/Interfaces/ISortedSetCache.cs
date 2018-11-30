using System;
using System.Collections.Generic;
using System.Text;
using Yunyong.Redis.Base;

namespace Yunyong.Redis.Interfaces
{
    public interface ISortedSetCache
        :Base.ICache
    {
        long Length(string key);

        bool Insert<T>(string key, T value, int sortFlag)
            where T : CacheValueBase;

        bool Delete(params string[] keys);

        bool Delete<T>(string key, T value)
            where T : CacheValueBase;

        List<T> Select<T>(string key, int sortFlag)
            where T : CacheValueBase;

        List<T> SelectPaged<T>(string key, int pageSize, int skipPage, string filterStr)
            where T : CacheValueBase;
    }
}
