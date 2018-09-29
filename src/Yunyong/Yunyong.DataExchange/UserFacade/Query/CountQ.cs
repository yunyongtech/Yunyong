using System.Threading.Tasks;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Impls;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.UserFacade.Query
{
    public class CountQ<M> 
        : Operator, IQuerySingleValue<M>
    {

        internal CountQ(Context dc)
            : base(dc)
        { }

        /// <summary>
        /// 单表单值查询
        /// </summary>
        /// <typeparam name="V">int/long/decimal/...</typeparam>
        public async Task<V> QuerySingleValueAsync<V>()
        {
            return await new QuerySingleValueImpl<M>(DC).QuerySingleValueAsync<V>();
        }

    }
}
