using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;

namespace Yunyong.DataExchange.UserFacade.Join
{
    public class FromX: Operator
    {
        internal FromX(DbContext dc)
        {
            DC = dc;
        }

        public JoinX From<M>(Expression<Func<M>> func)
        {
            var dic = DC.EH.ExpressionHandle(func);
            dic.Action = ActionEnum.From;
            dic.Crud = CrudTypeEnum.Join;
            DC.AddConditions(dic);
            return new JoinX(DC);
        }
    }
}
