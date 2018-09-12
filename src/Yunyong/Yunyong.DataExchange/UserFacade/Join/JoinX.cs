﻿using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Common;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;

namespace Yunyong.DataExchange.UserFacade.Join
{
    public class JoinX: Operator
    {

        internal JoinX(DbContext dc)
        {
            DC = dc;
        }

        public OnX InnerJoin<M>(Expression<Func<M>> func)
        {
            var dic = DC.EH.ExpressionHandle(func);
            dic.Action = ActionEnum.InnerJoin;
            dic.Crud = CrudTypeEnum.Join;
            DC.AddConditions(dic);
            return new OnX(DC);
        }

        public OnX LeftJoin<M>(out M m,string alias)
        {
            m = Activator.CreateInstance<M>();
            DC.AddConditions(new DicModel
            {
                TableOne = DC.SqlProvider.GetTableName(m),
                AliasOne = alias,
                Action = ActionEnum.LeftJoin,
                Crud = CrudTypeEnum.Join
            });
            return new OnX(DC);
        }

        public QueryFilter Where(Expression<Func<bool>> func)
        {
            var field = DC.EH.ExpressionHandle(func, ActionEnum.Where);
            field.Crud = CrudTypeEnum.Join;
            DC.AddConditions(field);
            return new QueryFilter(DC);
        }

    }
}
