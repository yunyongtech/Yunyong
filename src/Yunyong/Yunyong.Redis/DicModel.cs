using Yunyong.Redis.Base;

namespace Yunyong.Redis
{
    public class DicModel<T>
        where T : CacheValueBase
    {
        public string Key { get; set; }
        public T Value { get; set; }
    }
}
