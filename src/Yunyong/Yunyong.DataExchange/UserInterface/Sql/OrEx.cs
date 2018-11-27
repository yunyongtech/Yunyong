using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.UserFacade.Delete;
using Yunyong.DataExchange.UserFacade.Join;
using Yunyong.DataExchange.UserFacade.Query;
using Yunyong.DataExchange.UserFacade.Update;

namespace Yunyong.DataExchange
{
    public static class OrEx
    {

        /****************************************************************************************************************************************/

        /// <summary>
        /// 请参阅: <see langword=".Where() &amp; .And() &amp; .Or() 使用 " cref="https://www.cnblogs.com/Meng-NET/"/>
        /// </summary>
        public static WhereD<M> Or<M>(this WhereD<M> where, Expression<Func<M, bool>> compareFunc)
            where M : class
        {
            where.DC.Action = ActionEnum.Or;
            where.OrHandle(compareFunc);
            return where;
        }

        /****************************************************************************************************************************************/

        /// <summary>
        /// 请参阅: <see langword=".Where() &amp; .And() &amp; .Or() 使用 " cref="https://www.cnblogs.com/Meng-NET/"/>
        /// </summary>
        public static WhereU<M> Or<M>(this WhereU<M> where, Expression<Func<M, bool>> compareFunc)
            where M : class
        {
            where.DC.Action = ActionEnum.Or;
            where.OrHandle(compareFunc);
            return where;
        }

        /****************************************************************************************************************************************/

        /// <summary>
        /// 请参阅: <see langword=".Where() &amp; .And() &amp; .Or() 使用 " cref="https://www.cnblogs.com/Meng-NET/"/>
        /// </summary>
        public static WhereQ<M> Or<M>(this WhereQ<M> where, Expression<Func<M, bool>> compareFunc)
            where M : class
        {
            where.DC.Action = ActionEnum.Or;
            where.OrHandle(compareFunc);
            return where;
        }

        /****************************************************************************************************************************************/

        /// <summary>
        /// 请参阅: <see langword=".Where() &amp; .And() &amp; .Or() 使用 " cref="https://www.cnblogs.com/Meng-NET/"/>
        /// </summary>
        public static WhereX Or(this WhereX where, Expression<Func<bool>> compareFunc)
        {
            where.DC.Action = ActionEnum.Or;
            where.WhereJoinHandle(where, compareFunc);
            return where;
        }

    }
}
