namespace Yunyong.Core
{
    /// <summary>
    ///     分页查询设置
    /// </summary>
    public abstract class PagingQueryOption : QueryOption
    {
        public PagingQueryOption()
        {
            PageSize = PagingSetting.DefaultPageSize;
        }

        /// <summary>
        ///     当前页
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        ///     页面大小
        /// </summary>
        public int PageSize { get; set; }
    }
}