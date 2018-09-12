namespace Yunyong.Core
{
    /// <summary>
    ///     AsyncTaskStatus
    /// </summary>
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum AsyncTaskStatus
    {
        /// <summary>
        ///     成功
        /// </summary>
        Success,
        //IOException,

        /// <summary>
        ///     失败
        /// </summary>
        Failed
    }
}