using System;

namespace Yunyong.Redis
{
    public class CacheKey
    {
        public static string Key(Guid key)
        {
            return $"{Common.Redis.Main}:{Common.Redis.Sub}:{key.ToString()}";
        }
    }   
}
