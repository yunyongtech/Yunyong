using StackExchange.Redis;

namespace Yunyong.Cache.Redis
{
    public interface IRedisCacheDatabaseProvider
    {
        /// <summary>
        ///     获取默认库
        /// </summary>
        /// <returns></returns>
        IDatabase GetDatabase();

        /// <summary>
        ///     获取指定库
        /// </summary>
        /// <param name="cacheDatabaseType"></param>
        /// <returns></returns>
        IDatabase GetDatabase(CacheDatabaseEnum cacheDatabaseType);
    }
}