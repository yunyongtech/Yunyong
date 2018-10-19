﻿using System.Collections.Generic;

namespace Yunyong.Core
{
    /// <summary>
    ///     分页列表
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PagingList<TEntity>
    {
        /// <summary>
        ///     当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     页面大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     条目总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///     页面总数
        /// </summary>
        //public int TotalPage { get; set; }
        public int TotalPage
        {
            get
            {
                var totalPage = TotalCount / PageSize;
                if (TotalCount % PageSize > 0)
                {
                    ++totalPage;
                }

                return totalPage;
            }
        }

        /// <summary>
        ///     数据
        /// </summary>
        public List<TEntity> Data { get; set; }
    }
}