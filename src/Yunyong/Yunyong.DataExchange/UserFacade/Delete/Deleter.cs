using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;

namespace Yunyong.DataExchange.UserFacade.Delete
{
    public class Deleter<M>: Operator
    {
        internal Deleter(DbContext dc)
        {
            DC = dc;
        }

        /// <summary>
        /// 过滤条件起点
        /// </summary>
        /// <param name="func">格式: it => it.Id == m.Id </param>
        public DeleteFilter<M> Where(Expression<Func<M, bool>> func)
        {
            WhereHandle(func, CrudTypeEnum.Delete);
            return new DeleteFilter<M>(DC);
        }


    }
}
