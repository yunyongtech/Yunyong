using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Enums;
using Yunyong.DataExchange.UserFacade.Update;

namespace Yunyong.DataExchange
{
    public static class SetExtension
    {

        /// <summary>
        /// set 单个字段数据
        /// </summary>
        /// <param name="func">格式: it => it.CreatedOn</param>
        /// <param name="newVal">新值</param>
        public static Setter<M> Set<M, F>(this Setter<M> setter, Expression<Func<M, F>> func, F newVal)
        {
            setter.DC.OP.SetChangeHandle<M, F>(func, newVal, ActionEnum.Update, OptionEnum.Set);
            return setter;
        }

        /// <summary>
        /// set 多个字段数据
        /// </summary>
        public static Setter<M> Set<M>(this Setter<M> setter, object mSet)
        {
            setter.DC.OP.SetDynamicHandle<M>(mSet);
            return setter;
        }


        /// <summary>
        /// set 单个字段变更
        /// </summary>
        /// <param name="func">格式: it => it.LockedCount</param>
        /// <param name="modifyVal">变更值</param>
        /// <param name="change">+/-/...</param>
        public static Setter<M> Change<M, F>(this Setter<M> setter, Expression<Func<M, F>> func, F modifyVal, ChangeEnum change)
        {
            setter.DC.OP.SetChangeHandle<M, F>(func, modifyVal, ActionEnum.Update, setter.DC.SqlProvider.GetChangeOption(change));
            return setter;
        }


    }
}
