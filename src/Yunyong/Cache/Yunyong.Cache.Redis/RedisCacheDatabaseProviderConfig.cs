namespace Yunyong.Cache.Redis
{
    public class RedisCacheDatabaseProviderConfig
    {
        public string ConnectionString { get; set; }

        public int DatabaseId { get; set; } = -1;
    }
}