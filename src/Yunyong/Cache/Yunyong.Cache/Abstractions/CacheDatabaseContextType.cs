namespace Yunyong.Cache.Abstractions
{
    /// <summary>
    ///     缓存数据库上下文类型
    /// </summary>
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum CacheDatabaseContextType
    {
        /// <summary>
        ///     默认库
        /// </summary>
        Default = 0,

        /// <summary>
        ///     统计库
        /// </summary>
        Statistics = 1
    }
}