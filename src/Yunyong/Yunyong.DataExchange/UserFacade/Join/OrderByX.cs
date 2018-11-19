﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.Core;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Impls;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.UserFacade.Join
{
    public class OrderByX
        : Operator, IFirstOrDefaultX, IListX, IPagingListX, IPagingListXO,ITopX
    {
        internal OrderByX(Context dc) 
            : base(dc)
        {
        }

        /// <summary>
        /// 多表单条数据查询
        /// </summary>
        public async Task<M> FirstOrDefaultAsync<M>()
            where M:class
        {
            return await new FirstOrDefaultXImpl(DC).FirstOrDefaultAsync<M>();
        }
        /// <summary>
        /// 多表单条数据查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        public async Task<VM> FirstOrDefaultAsync<VM>(Expression<Func<VM>> columnMapFunc)
            where VM:class
        {
            return await new FirstOrDefaultXImpl(DC).FirstOrDefaultAsync<VM>(columnMapFunc);
        }

        /// <summary>
        /// 多表多条数据查询
        /// </summary>
        public async Task<List<M>> ListAsync<M>()
            where M:class
        {
            return await new ListXImpl(DC).ListAsync<M>();
        }
        /// <summary>
        /// 多表多条数据查询
        /// </summary>
        public async Task<List<VM>> ListAsync<VM>(Expression<Func<VM>> columnMapFunc)
            where VM:class
        {
            return await new ListXImpl(DC).ListAsync<VM>(columnMapFunc);
        }
        /// <summary>
        /// 多表多条数据查询
        /// </summary>
        public async Task<List<M>> ListAsync<M>(int topCount)
            where M : class
        {
            return await new ListXImpl(DC).ListAsync<M>(topCount);
        }
        /// <summary>
        /// 多表多条数据查询
        /// </summary>
        public async Task<List<VM>> ListAsync<VM>(int topCount, Expression<Func<VM>> columnMapFunc)
            where VM : class
        {
            return await new ListXImpl(DC).ListAsync<VM>(topCount, columnMapFunc);
        }

        /// <summary>
        /// 多表分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<M>> PagingListAsync<M>(int pageIndex, int pageSize)
            where M:class
        {
            return await new PagingListXImpl(DC).PagingListAsync<M>(pageIndex, pageSize);
        }
        /// <summary>
        /// 多表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<VM>> PagingListAsync<VM>(int pageIndex, int pageSize, Expression<Func<VM>> columnMapFunc)
            where VM:class
        {
            return await new PagingListXImpl(DC).PagingListAsync<VM>(pageIndex, pageSize, columnMapFunc);
        }

        /// <summary>
        /// 多表分页查询
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<M>> PagingListAsync<M>(PagingQueryOption option)
            where M:class
        {
            return await new PagingListXOImpl(DC).PagingListAsync<M>(option);
        }
        /// <summary>
        /// 多表分页查询
        /// </summary>
        /// <typeparam name="VM">ViewModel</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        public async Task<PagingList<VM>> PagingListAsync<VM>(PagingQueryOption option, Expression<Func<VM>> columnMapFunc)
            where VM:class
        {
            return await new PagingListXOImpl(DC).PagingListAsync<VM>(option, columnMapFunc);
        }

        /// <summary>
        /// 多表多条数据查询
        /// </summary>
        public async Task<List<M>> TopAsync<M>(int count)
            where M : class
        {
            return await new TopXImpl(DC).TopAsync<M>(count);
        }
        /// <summary>
        /// 多表多条数据查询
        /// </summary>
        public async Task<List<VM>> TopAsync<VM>(int count, Expression<Func<VM>> columnMapFunc)
            where VM : class
        {
            return await new TopXImpl(DC).TopAsync<VM>(count, columnMapFunc);
        }
    }
}
