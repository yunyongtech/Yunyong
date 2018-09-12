namespace Yunyong.EventBus.EasyNetQ
{
    /// <summary>
    ///     MongoDB连接配置
    /// </summary>
    public class MongoDBConfig
    {
        /// <summary>
        ///     MongoDB连接配置
        /// </summary>
        /// <value>
        ///     The connection string.
        /// </value>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     所采用MongoDB数据库名称
        /// </summary>
        public string MongoDB { get; set; }
    }
}