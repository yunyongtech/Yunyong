﻿using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.UserFacade.Query;

namespace Yunyong.DataExchange
{
    public static class OrderByExtension
    {

        /**************************************************************************************************************/

        public static OrderByQ<M> OrderBy<M, F>(this WhereQ<M> where, Expression<Func<M,F>> func,OrderByEnum orderBy= OrderByEnum.Desc)
        {
            where.DC.OP.OrderByHandle(func, orderBy);
            return new OrderByQ<M>(where.DC);
        }

        public static ThenOrderByQ<M> ThenOrderBy<M, F>(this OrderByQ<M> orderByQ, Expression<Func<M, F>> func, OrderByEnum orderBy = OrderByEnum.Desc)
        {
            orderByQ.DC.OP.OrderByHandle(func, orderBy);
            return new ThenOrderByQ<M>(orderByQ.DC);
        }

        /**************************************************************************************************************/

    }
}
