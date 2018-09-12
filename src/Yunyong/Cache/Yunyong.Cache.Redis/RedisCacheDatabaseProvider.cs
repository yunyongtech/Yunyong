using StackExchange.Redis;

namespace Yunyong.Cache.Redis
{
    /// <summary>
    ///     RedisDatabaseProvider
    /// </summary>
    public class RedisCacheDatabaseProvider : IRedisCacheDatabaseProvider
    {
        #region Member

        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisCacheDatabaseProviderConfig _redisCacheDatabaseProviderConfig;

        public RedisCacheDatabaseProvider(RedisCacheDatabaseProviderConfig redisCacheDatabaseProviderConfig)
        {
            _redisCacheDatabaseProviderConfig = redisCacheDatabaseProviderConfig;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_redisCacheDatabaseProviderConfig.ConnectionString);
        }

        #endregion

        #region Service

        /// <summary>
        ///     获取默认库
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase(_redisCacheDatabaseProviderConfig.DatabaseId);
        }

        /// <summary>
        ///     获取指定库
        /// </summary>
        /// <param name="cacheDatabaseType"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(CacheDatabaseEnum cacheDatabaseType)
        {
            return _connectionMultiplexer.GetDatabase((int) cacheDatabaseType);
        }

        #endregion
    }
}