using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.UserFacade.Join;

namespace Yunyong.DataExchange
{
    public static class JoinEx
    {

        /***************************************************************************************************************************/

        public static JoinX InnerJoin<M>(this FromX from, Expression<Func<M>> modelFunc)
        {
            from.DC.Action = ActionEnum.InnerJoin;
            var dic = from.DC.EH.FuncTExpression(modelFunc);
            from.DC.DPH.AddParameter(dic);
            return new JoinX(from.DC);
        }

        public static JoinX LeftJoin<M>(this FromX from, Expression<Func<M>> modelFunc)
        {
            from.DC.Action = ActionEnum.LeftJoin;
            var dic = from.DC.EH.FuncTExpression(modelFunc);
            from.DC.DPH.AddParameter(dic);
            return new JoinX(from.DC);
        }

        /***************************************************************************************************************************/

        public static JoinX InnerJoin<M>(this OnX on, Expression<Func<M>> modelFunc)
        {
            on.DC.Action = ActionEnum.InnerJoin;
            var dic = on.DC.EH.FuncTExpression(modelFunc);
            on.DC.DPH.AddParameter(dic);
            return new JoinX(on.DC);
        }

        public static JoinX LeftJoin<M>(this OnX on, Expression<Func<M>> modelFunc)
        {
            on.DC.Action = ActionEnum.LeftJoin;
            var dic = on.DC.EH.FuncTExpression(modelFunc);
            on.DC.DPH.AddParameter(dic);
            return new JoinX(on.DC);
        }

    }
}
