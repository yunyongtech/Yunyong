using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using Yunyong.Redis.Base;
using Yunyong.Redis.Interfaces;

namespace Yunyong.Redis
{
    public class StringCache
        : Base.Cache, IStringCache
    {
        public StringCache(double ValidTimeSpan)
            : base(ValidTimeSpan)
        { }

        public bool Insert<T>(string key, T value)
            where T : CacheValueBase
        {
            return Insert(new DicModel<T>
            {
                Key = key,
                Value = value
            });
        }

        public bool Insert<T>(params DicModel<T>[] dics)
            where T : CacheValueBase
        {
            //
            if (dics == null
                || dics.Length <= 0)
            {
                return false;
            }

            //
            var flag = false;
            flag = _DB.StringSet
            (
                dics
                .Select(it =>
                {
                    it.Value.Key = it.Key;
                    return new KeyValuePair<RedisKey, RedisValue>(it.Key, JsonConvert.SerializeObject(it.Value));
                })
                .ToArray()
            );
            if (flag == true)
            {
                flag = Expire(_defaultValidTimeSpan, dics.Select(it => it.Key).ToArray());
            }
            return flag;
        }

        public bool Delete(params string[] keys)
        {
            return Delete(RedisType.String, keys);
        }

        public bool Update<T>(string key, T value)
            where T : CacheValueBase
        {
            return Update(new DicModel<T>
            {
                Key = key,
                Value = value
            });
        }

        public bool Update<T>(params DicModel<T>[] dics)
            where T : CacheValueBase
        {
            //
            if (dics == null
                || dics.Length <= 0)
            {
                return false;
            }

            //
            var flag = false;
            flag = Delete(dics.Select(it => it.Key).ToArray());
            if (flag)
            {
                flag = Insert(dics);
            }
            return flag;
        }

        public T Select<T>(string key)
            where T : CacheValueBase
        {
            return Select<T>(new string[] { key })?.First()?.Value;
        }

        public List<DicModel<T>> Select<T>(params string[] keys)
            where T : CacheValueBase
        {
            //
            var results = _DB.StringGet
            (
                keys
                .Select(it => (RedisKey)it)
                .ToArray()
             );

            //
            var dics = new List<DicModel<T>>();
            if (results != null
                && results.Count() > 0)
            {
                foreach (var it in keys)
                {
                    var dic = new DicModel<T>();
                    dic.Key = it;
                    var value = results.FirstOrDefault(r =>
                    {
                        if (!r.IsNull
                            && r.HasValue)
                        {
                            return ((string)r).Contains(it);
                        }
                        else
                        {
                            return false;
                        }
                    });
                    if (!value.IsNull
                        && value.HasValue)
                    {
                        dic.Value = JsonConvert.DeserializeObject<T>(value);
                    }
                    dics.Add(dic);
                }
            }
            return dics;
        }

    }
}
