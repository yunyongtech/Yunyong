using Newtonsoft.Json;
using Yunyong.Redis.Base;
using Yunyong.Redis.Interfaces;

namespace Yunyong.Redis
{
    public class StackCache
        : ListCache, IStackCache
    {
        public StackCache(double ValidTimeSpan)
            : base(ValidTimeSpan)
        { }

        public bool StackPush<T>(string key, T value)
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
            var flag = _DB.ListRightPush(key, JsonConvert.SerializeObject(value)) > 0;
            if (flag)
            {
                flag = Expire(_defaultValidTimeSpan, key);
            }
            return flag;
        }

        public T StackPop<T>(string key)
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
