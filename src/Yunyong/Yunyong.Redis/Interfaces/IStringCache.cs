using System.Collections.Generic;
using Yunyong.Redis.Base;

namespace Yunyong.Redis.Interfaces
{
    public interface IStringCache
        : ICache
    {
        bool Insert<T>(string key, T value)
             where T : CacheValueBase;
        bool Insert<T>(params DicModel<T>[] dics)
            where T : CacheValueBase;

        bool Delete(params string[] keys);

        bool Update<T>(string key, T value)
            where T : CacheValueBase;
        bool Update<T>(params DicModel<T>[] dics)
           where T : CacheValueBase;

        T Select<T>(string key)
            where T : CacheValueBase;
        List<DicModel<T>> Select<T>(params string[] keys)
             where T : CacheValueBase;

    }
}
