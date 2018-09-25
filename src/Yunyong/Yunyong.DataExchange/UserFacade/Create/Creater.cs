using System.Collections.Generic;
using System.Threading.Tasks;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;
using Yunyong.DataExchange.Helper;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.UserFacade.Create
{
    public class Creater<M> 
        : Operator, ICreate<M>, ICreateBatch<M>
    {
        internal Creater(Context dc) 
            : base(dc)
        { }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <returns>插入条目数</returns>
        public async Task<int> CreateAsync(M m)
        {
            DC.GetProperties(m);
            return await SqlHelper.ExecuteAsync(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.CreateAsync)[0],
                DC.GetParameters());
        }
        
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <returns>插入条目数</returns>
        public async Task<int> CreateBatchAsync(IEnumerable<M> mList)
        {
            return await DC.BDH.StepProcess(mList, 15, async list =>
            {
                DC.ResetConditions();
                DC.GetProperties(list);                
                return await SqlHelper.ExecuteAsync(
                    DC.Conn,
                    DC.SqlProvider.GetSQL<M>(UiMethodEnum.CreateBatchAsync)[0],
                    DC.GetParameters());
            });
        }

    }
}
