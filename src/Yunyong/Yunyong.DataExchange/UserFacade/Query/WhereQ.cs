using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.Core;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Impls;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.UserFacade.Query
{
    public sealed class WhereQ<M>
        : Operator, IExist, IQueryFirstOrDefault<M>, IQueryList<M>, IQueryPagingList<M>, IQueryPagingListO<M>, ICount<M>,ITop<M>,ISum<M>
        where M : class
    {
        internal WhereQ(Context dc)
            : base(dc)
        { }

        /// <summary>
        /// 查询是否存在符合条件的数据
        /// </summary>
        public async Task<bool> ExistAsync()
        {
            return await new ExistImpl<M>(DC).ExistAsync();
        }

        /// <summary>
        /// 查询符合条件数据条目数
        /// </summary>
        public async Task<long> CountAsync()
        {
            return await new CountImpl<M>(DC).CountAsync();
        }
        /// <summary>
        /// 查询符合条件数据条目数
        /// </summary>
        public async Task<long> CountAsync<F>(Expression<Func<M, F>> propertyFunc)
        {
            return await new CountImpl<M>(DC).CountAsync(propertyFunc);
        }

        /// <summary>
        /// 列求和 -- select sum(col) from ...
        /// </summary>
        public async Task<F> SumAsync<F>(Expression<Func<M, F>> func)
            where F : struct
        {
            return await new SumImpl<M>(DC).SumAsync(func);
        }

        /// <summary>
        /// 单表单条数据查询
        /// </summary>
        public async Task<M> QueryFirstOrDefaultAsync()
        {
            return await new QueryFirstOrDefaultImpl<M>(DC).QueryFirstOrDefaultAsync();
        }
        /// <summary>
        /// 单表单条数据查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        public async Task<VM> QueryFirstOrDefaultAsync<VM>()
            where VM:class
        {
            return await new QueryFirstOrDefaultImpl<M>(DC).QueryFirstOrDefaultAsync<VM>();
        }
        /// <summary>
        /// 单表单条数据查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        public async Task<VM> QueryFirstOrDefaultAsync<VM>(Expression<Func<M, VM>> columnMapFunc)
            where VM:class
        {
            return await new QueryFirstOrDefaultImpl<M>(DC).QueryFirstOrDefaultAsync<VM>(columnMapFunc);
        }

        /// <summary>
        /// 单表多条数据查询
        /// </summary>
        public async Task<List<M>> QueryListAsync()
        {
            return await new QueryListImpl<M>(DC).QueryListAsync();
        }
        /// <summary>
        /// 单表多条数据查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        public async Task<List<VM>> QueryListAsync<VM>()
            where VM:class
        {
            return await new QueryListImpl<M>(DC).QueryListAsync<VM>();
        }
        /// <summary>
        /// 单表多条数据查询
        /// </summary>
        public async Task<List<VM>> QueryListAsync<VM>(Expression<Func<M, VM>> columnMapFunc)
            where VM:class
        {
            return await new QueryListImpl<M>(DC).QueryListAsync<VM>(columnMapFunc);
        }
        /// <summary>
        /// 单表多条数据查询
        /// </summary>
        public async Task<List<M>> QueryListAsync(int topCount)
        {
            return await new QueryListImpl<M>(DC).QueryListAsync(topCount);
        }
        /// <summary>
        /// 单表多条数据查询
        /// </summary>
        public async Task<List<VM>> QueryListAsync<VM>(int topCount) 
            where VM : class
        {
            return await new QueryListImpl<M>(DC).QueryListAsync<VM>(topCount);
        }
        /// <summary>
        /// 单表多条数据查询
        /// </summary>
        public async Task<List<VM>> QueryListAsync<VM>(int topCount, Expression<Func<M, VM>> columnMapFunc) 
            where VM : class
        {
            return await new QueryListImpl<M>(DC).QueryListAsync<VM>(topCount, columnMapFunc);
        }

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<M>> QueryPagingListAsync(int pageIndex, int pageSize)
        {
            return await new QueryPagingListImpl<M>(DC).QueryPagingListAsync(pageIndex, pageSize);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<VM>> QueryPagingListAsync<VM>(int pageIndex, int pageSize)
            where VM:class
        {
            return await new QueryPagingListImpl<M>(DC).QueryPagingListAsync<VM>(pageIndex, pageSize);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<VM>> QueryPagingListAsync<VM>(int pageIndex, int pageSize, Expression<Func<M, VM>> columnMapFunc)
            where VM:class
        {
            return await new QueryPagingListImpl<M>(DC).QueryPagingListAsync<VM>(pageIndex, pageSize, columnMapFunc);
        }

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<M>> QueryPagingListAsync(PagingQueryOption option)
        {
            return await new QueryPagingListOImpl<M>(DC).QueryPagingListAsync(option);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<VM>> QueryPagingListAsync<VM>(PagingQueryOption option)
            where VM:class
        {
            return await new QueryPagingListOImpl<M>(DC).QueryPagingListAsync<VM>(option);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<VM>> QueryPagingListAsync<VM>(PagingQueryOption option, Expression<Func<M, VM>> columnMapFunc)
            where VM:class
        {
            return await new QueryPagingListOImpl<M>(DC).QueryPagingListAsync<VM>(option, columnMapFunc);
        }

        /// <summary>
        /// 单表数据查询
        /// </summary>
        /// <param name="count">top count</param>
        /// <returns>返回 top count 条数据</returns>
        public async Task<List<M>> TopAsync(int count)
        {
            return await new TopImpl<M>(DC).TopAsync(count);
        }
        /// <summary>
        /// 单表数据查询
        /// </summary>
        /// <param name="count">top count</param>
        /// <returns>返回 top count 条数据</returns>
        public async Task<List<VM>> TopAsync<VM>(int count)
            where VM : class
        {
            return await new TopImpl<M>(DC).TopAsync<VM>(count);
        }
        /// <summary>
        /// 单表数据查询
        /// </summary>
        /// <param name="count">top count</param>
        /// <returns>返回 top count 条数据</returns>
        public async Task<List<VM>> TopAsync<VM>(int count, Expression<Func<M, VM>> columnMapFunc)
            where VM : class
        {
            return await new TopImpl<M>(DC).TopAsync<VM>(count, columnMapFunc);
        }


    }
}
