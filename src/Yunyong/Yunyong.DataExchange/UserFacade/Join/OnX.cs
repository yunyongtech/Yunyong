using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;

namespace Yunyong.DataExchange.UserFacade.Join
{
    public class OnX: Operator
    {

        internal OnX(DbContext dc)
        {
            DC = dc;
        }

        public JoinX On(Expression<Func<bool>> func)
        {
            var field = DC.EH.ExpressionHandle(func, ActionEnum.On);
            field.Crud = CrudTypeEnum.Join;
            DC.AddConditions(field);
            return new JoinX(DC);
        }

    }
}
