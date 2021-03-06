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
    public sealed class Queryer<M>
        : Operator, IAll<M>, IPagingAll<M>, ITop<M>
        where M : class
    {
        internal Queryer(Context dc)
            : base(dc)
        { }

        /// <summary>
        /// 单表数据查询
        /// </summary>
        /// <returns>返回全表数据</returns>
        public async Task<List<M>> AllAsync()
        {
            return await new AllImpl<M>(DC).AllAsync();
        }
        /// <summary>
        /// 单表数据查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <returns>返回全表数据</returns>
        public async Task<List<VM>> AllAsync<VM>()
            where VM : class
        {
            return await new AllImpl<M>(DC).AllAsync<VM>();
        }
        public async Task<List<F>> AllAsync<F>(Expression<Func<M, F>> propertyFunc)
        {
            return await new AllImpl<M>(DC).AllAsync<F>(propertyFunc);
        }

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回全表分页数据</returns>
        public async Task<PagingList<M>> PagingAllAsync(int pageIndex, int pageSize)
        {
            return await new PagingAllImpl<M>(DC).PagingAllAsync(pageIndex, pageSize);
        }
        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns>返回全表分页数据</returns>
        public async Task<PagingList<VM>> PagingAllAsync<VM>(int pageIndex, int pageSize)
            where VM : class
        {
            return await new PagingAllImpl<M>(DC).PagingAllAsync<VM>(pageIndex, pageSize);
        }
        public async Task<PagingList<T>> PagingAllAsync<T>(int pageIndex, int pageSize, Expression<Func<M, T>> columnMapFunc)
        {
            return await new PagingAllImpl<M>(DC).PagingAllAsync<T>(pageIndex, pageSize, columnMapFunc);
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
        public async Task<List<T>> TopAsync<T>(int count, Expression<Func<M, T>> columnMapFunc) 
        {
            return await new TopImpl<M>(DC).TopAsync<T>(count, columnMapFunc);
        }
    }
}
