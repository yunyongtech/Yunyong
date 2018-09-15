using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.Core;
using Yunyong.DataExchange.Common;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;
using Yunyong.DataExchange.Helper;

namespace Yunyong.DataExchange.UserFacade.Join
{
    public class WhereX : Operator, IMethodObject
    {

        internal WhereX(Context dc)
            : base(dc)
        { }


        /// <summary>
        /// 多表单条数据查询
        /// </summary>
        public async Task<M> QueryFirstOrDefaultAsync<M>()
        {
            SelectMHandle<M>();
            return await SqlHelper.QueryFirstOrDefaultAsync<M>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.JoinQueryFirstOrDefaultAsync)[0],
                DC.GetParameters());
        }
        /// <summary>
        /// 单表单条数据查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        public async Task<VM> QueryFirstOrDefaultAsync<VM>(Expression<Func<VM>> func)
        {
            SelectMHandle(func);
            return await SqlHelper.QueryFirstOrDefaultAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<VM>(UiMethodEnum.JoinQueryFirstOrDefaultAsync)[0],
                DC.GetParameters());
        }


        public async Task<List<M>> QueryListAsync<M>()
        {
            SelectMHandle<M>();
            return (await SqlHelper.QueryAsync<M>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.JoinQueryListAsync)[0],
                DC.GetParameters())).ToList();
        }
        public async Task<List<VM>> QueryListAsync<VM>(Expression<Func<VM>> func)
        {
            SelectMHandle(func);
            return (await SqlHelper.QueryAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<VM>(UiMethodEnum.JoinQueryListAsync)[0],
                DC.GetParameters())).ToList();
        }

        /// <summary>
        /// 多表分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<M>> QueryPagingListAsync<M>(int pageIndex, int pageSize)
        {
            var result = new PagingList<M>();
            result.PageIndex = pageIndex;
            result.PageSize = pageSize;
            var paras = DC.GetParameters();
            var sql = DC.SqlProvider.GetSQL<M>(UiMethodEnum.JoinQueryPagingListAsync, result.PageIndex, result.PageSize);
            result.TotalCount = await SqlHelper.ExecuteScalarAsync<int>(DC.Conn, sql[0], paras);
            result.Data = (await SqlHelper.QueryAsync<M>(DC.Conn, sql[1], paras)).ToList();
            return result;
        }
        ///// <summary>
        ///// 单表分页查询
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">每页条数</param>
        //public async Task<PagingList<M>> QueryPagingListAsync(PagingQueryOption option)
        //{
        //    OrderByOptionHandle(option);
        //    return await QueryPagingListAsyncHandle<M, M>(option.PageIndex, option.PageSize, UiMethodEnum.QueryPagingListAsync);
        //}
        ///// <summary>
        ///// 单表分页查询
        ///// </summary>
        ///// <typeparam name="VM">ViewModel</typeparam>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">每页条数</param>
        //public async Task<PagingList<VM>> QueryPagingListAsync<VM>(int pageIndex, int pageSize)
        //{
        //    return await QueryPagingListAsyncHandle<M, VM>(pageIndex, pageSize, UiMethodEnum.QueryPagingListAsync);
        //}
        ///// <summary>
        ///// 单表分页查询
        ///// </summary>
        ///// <typeparam name="VM">ViewModel</typeparam>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">每页条数</param>
        //public async Task<PagingList<VM>> QueryPagingListAsync<VM>(PagingQueryOption option)
        //{
        //    OrderByOptionHandle(option);
        //    return await QueryPagingListAsyncHandle<M, VM>(option.PageIndex, option.PageSize, UiMethodEnum.QueryPagingListAsync);
        //}

    }
}
