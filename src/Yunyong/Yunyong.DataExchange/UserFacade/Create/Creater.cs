﻿using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;
using Yunyong.DataExchange.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yunyong.DataExchange.Common;

namespace Yunyong.DataExchange.UserFacade.Create
{
    public class Creater<M>: Operator,IMethodObject
    {
        internal Creater(DbContext dc)
        {
            DC = dc;
            DC.OP = this;
        }


        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <returns>插入条目数</returns>
        public async Task<int> CreateAsync(M m)
        {
            await DC.GetProperties(m);
            return await SqlHelper.ExecuteAsync(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(SqlTypeEnum.CreateAsync)[0],
                DC.GetParameters());
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <returns>插入条目数</returns>
        public async Task<int> CreateBatchAsync(IEnumerable<M> mList)
        {
            await DC.GetProperties(mList);
            return await SqlHelper.ExecuteAsync(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(SqlTypeEnum.CreateBatchAsync)[0],
                DC.GetParameters());
        }

    }
}