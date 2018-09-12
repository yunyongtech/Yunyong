namespace Yunyong.EventBus
{
    /// <summary>
    ///     Event Bus配置
    /// </summary>
    public class EventBusConfig
    {
        //"HostName": "127.0.0.1",
        //"UserName": "guest",
        //"Password": "guest",
        //"RetryCount": "5"
        /// <summary>
        ///     EventBus 服务器
        /// </summary>
        public string HostName { get; set; }

        public string VirtualHost { get; set; } = "/";

        /// <summary>
        ///     Event Bus登录名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Event Bus登陆密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     预取数量
        /// </summary>
        public int PrefetchCount { get; set; } = 1;

        /// <summary>
        ///     RetryCount
        /// </summary>
        public int RetryCount { get; set; }

        public string Prefix { get; set; }
        //public bool AopInject { get; set; } = true;
    }
}