namespace Yunyong.Cache
{
    /// <summary>
    ///     缓存配置
    /// </summary>
    public class CacheServiceConfig
    {
        /// <summary>
        ///     缓存服务地址
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     服务端口
        /// </summary>
        public int Port { get; set; } = 6379;

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 0 ~ 15
        /// </summary>
        public int DB { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// 功能
        /// </summary>
        public string Function { get; set; }
    }
}