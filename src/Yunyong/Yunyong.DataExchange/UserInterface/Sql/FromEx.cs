using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.UserFacade.Join;

namespace Yunyong.DataExchange
{
    public static class FromEx
    {

        public static FromX From<M>(this Queryer join, Expression<Func<M>> tableModelFunc)
        {
            join.DC.Action = ActionEnum.From;
            var dic = join.DC.EH.FuncTExpression(tableModelFunc);
            join.DC.DPH.AddParameter(dic);
            return new FromX(join.DC);
        }

    }
}
