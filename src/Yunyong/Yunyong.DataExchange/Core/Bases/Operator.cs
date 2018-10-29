using System;
using System.Collections.Core.Extensions;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Yunyong.Core;
using Yunyong.DataExchange.Core.Common;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.Core.Bases
{
    public abstract class Operator
        : IObjectMethod
    {

        internal Operator(Context dc)
        {
            DC = dc;
            DC.OP = this;
        }

        private bool CheckWhereVal(object val, Type valType)
        {
            if (val == null
                || (valType.IsEnum && "0".Equals(val.ToString(), StringComparison.OrdinalIgnoreCase))
                || (valType == typeof(DateTime) && Convert.ToDateTime("0001-01-01 00:00:00.000000").Equals(val))
                || (valType.IsList() && (val as dynamic).Count <= 0)
                || (valType.IsArray && (val as dynamic).Length <= 0))
            {
                return false;
            }

            return true;
        }

        private List<(string key, string param, (object val, string valStr) val, Type valType, string colType, CompareEnum compare)> GetSetKPV<M>(object objx)
        {
            var list = new List<DicQueryModel>();
            var dic = default(IDictionary<string, object>);

            //
            var mProps = typeof(M).GetProperties();
            if (objx is ExpandoObject)
            {
                dic = objx as IDictionary<string, object>;
                foreach (var mp in mProps)
                {
                    foreach (var sp in dic.Keys)
                    {
                        if (mp.Name.Equals(sp, StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new DicQueryModel
                            {
                                MField = mp.Name,
                                VmField = mp.Name,
                                Compare = CompareEnum.Equal
                            });
                        }
                    }
                }
            }
            else
            {
                var oProps = objx.GetType().GetProperties();
                foreach (var mp in mProps)
                {
                    foreach (var sp in oProps)
                    {
                        if (mp.Name.Equals(sp.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new DicQueryModel
                            {
                                MField = mp.Name,
                                VmField = mp.Name,
                                Compare = CompareEnum.Equal
                            });
                        }
                    }
                }
            }

            //
            var result = new List<(string key, string param, (object val, string valStr) val, Type valType, string colType, CompareEnum compare)>();
            var columns = DC.SC.GetColumnInfos(DC.SC.GetModelKey(typeof(M).FullName));
            foreach (var prop in list)
            {
                var val = default((object val, string valStr));
                var valType = default(Type);
                var columnType = columns.First(it => it.ColumnName.Equals(prop.MField, StringComparison.OrdinalIgnoreCase)).DataType;
                if (objx is ExpandoObject)
                {
                    var obj = dic[prop.MField];
                    valType = obj.GetType();
                    val = DC.VH.ExpandoObjectValue(obj);
                    result.Add((prop.MField, prop.VmField, val, valType, columnType, prop.Compare));
                }
                else
                {
                    var mp = objx.GetType().GetProperty(prop.MField);
                    valType = mp.PropertyType;
                    val = DC.VH.PropertyValue(mp, objx);
                    result.Add((prop.MField, prop.VmField, val, valType, columnType, prop.Compare));
                }
            }
            return result;
        }
        private List<(string key, string param, (object val, string valStr) val, Type valType, string colType, CompareEnum compare)> GetWhereKPV<M>(object objx)
        {
            var list = new List<DicQueryModel>();
            var dic = default(IDictionary<string, object>);
            //
            var mProps = typeof(M).GetProperties();
            var oType = objx.GetType();
            if (objx is IQueryOption)
            {
                var oProps = oType.GetProperties(XConfig.ClassSelfMember);
                foreach (var mp in mProps)
                {
                    foreach (var sp in oProps)
                    {
                        var spAttr = DC.AH.GetAttribute<QueryColumnAttribute>(oType, sp) as QueryColumnAttribute;
                        var spName = string.Empty;
                        var compare = CompareEnum.Equal;
                        if (spAttr != null
                            && !string.IsNullOrWhiteSpace(spAttr.ColumnName))
                        {
                            spName = spAttr.ColumnName;
                            compare = spAttr.CompareCondition;
                        }
                        else
                        {
                            spName = sp.Name;
                        }

                        if (mp.Name.Equals(spName, StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new DicQueryModel
                            {
                                MField = mp.Name,
                                VmField = sp.Name,
                                Compare = compare
                            });
                        }
                    }
                }
            }
            else if (objx is ExpandoObject)
            {
                dic = objx as IDictionary<string, object>;
                foreach (var mp in mProps)
                {
                    foreach (var sp in dic.Keys)
                    {
                        if (mp.Name.Equals(sp, StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new DicQueryModel
                            {
                                MField = mp.Name,
                                VmField = mp.Name,
                                Compare = CompareEnum.Equal
                            });
                        }
                    }
                }
            }
            else
            {
                var oProps = oType.GetProperties();
                foreach (var mp in mProps)
                {
                    foreach (var sp in oProps)
                    {
                        if (mp.Name.Equals(sp.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            list.Add(new DicQueryModel
                            {
                                MField = mp.Name,
                                VmField = mp.Name,
                                Compare = CompareEnum.Equal
                            });
                        }
                    }
                }
            }

            //
            var result = new List<(string key, string param, (object val, string valStr) val, Type valType, string colType, CompareEnum compare)>();
            var columns = DC.SC.GetColumnInfos(DC.SC.GetModelKey(typeof(M).FullName));
            foreach (var prop in list)
            {
                var val = default((object val, string valStr));
                var valType = default(Type);
                var columnType = string.Empty;
                var xx = columns.FirstOrDefault(it => it.ColumnName.Equals(prop.MField, StringComparison.OrdinalIgnoreCase));
                if (xx != null)
                {
                    columnType = xx.DataType;
                }
                if (objx is IQueryOption)
                {
                    var mp = objx.GetType().GetProperty(prop.VmField);
                    valType = mp.PropertyType;
                    val = DC.VH.PropertyValue(mp, objx);
                    if (!CheckWhereVal(val.val, valType))
                    {
                        continue;
                    }
                    if (valType.IsList()
                        || valType.IsArray)
                    {
                        var ox = DC.VH.InValue(valType, val.val);
                        valType = ox.valType;
                        val = (ox.val, string.Empty);
                    }
                    result.Add((prop.MField, prop.VmField, val, valType, columnType, prop.Compare));
                }
                else if (objx is ExpandoObject)
                {
                    var obj = dic[prop.MField];
                    valType = obj.GetType();
                    val = DC.VH.ExpandoObjectValue(obj);
                    if (!CheckWhereVal(val.val, valType))
                    {
                        continue;
                    }
                    result.Add((prop.MField, prop.VmField, val, valType, columnType, prop.Compare));
                }
                else
                {
                    var mp = objx.GetType().GetProperty(prop.MField);
                    valType = mp.PropertyType;
                    val = DC.VH.PropertyValue(mp, objx);
                    if (!CheckWhereVal(val.val, valType))
                    {
                        continue;
                    }
                    result.Add((prop.MField, prop.VmField, val, valType, columnType, prop.Compare));
                }
            }
            return result;
        }

        /****************************************************************************************************************************************/

        internal Context DC { get; set; }

        /****************************************************************************************************************************************/

        internal void SetChangeHandle<M, F>(Expression<Func<M, F>> func, F modVal, OptionEnum option)
            where M : class
        {
            var keyDic = DC.EH.FuncMFExpression(func)[0];
            var key = keyDic.ColumnOne;
            var val = default((object val, string valStr));
            if (modVal == null)
            {
                val = (null, string.Empty);
            }
            else
            {
                val = DC.VH.ExpandoObjectValue(modVal);
            }
            DC.Option = option;
            DC.Compare = CompareEnum.None;
            DC.AddConditions(DC.DH.SetDic(typeof(M).FullName, key, key, val, typeof(F)));
        }

        internal void SetDynamicHandle<M>(object mSet)
        {
            var tuples = GetSetKPV<M>(mSet);
            var fullName = typeof(M).FullName;
            foreach (var tp in tuples)
            {
                DC.Option = OptionEnum.Set;
                DC.Compare = CompareEnum.None;
                DC.AddConditions(DC.DH.SetDic(fullName, tp.key, tp.param, tp.val, tp.valType));
            }
        }

        internal void WhereJoinHandle(Operator op, Expression<Func<bool>> func)
        {
            var dic = op.DC.EH.FuncBoolExpression(func);
            op.DC.AddConditions(dic);
        }

        internal void WhereHandle<M>(Expression<Func<M, bool>> func)
            where M : class
        {
            var field = DC.EH.FuncMBoolExpression(ActionEnum.Where, func);
            field.ClassFullName = typeof(M).FullName;
            field.Action = ActionEnum.Where;
            DC.AddConditions(field);
        }

        internal void WhereDynamicHandle<M>(object mWhere)
        {
            var tuples = GetWhereKPV<M>(mWhere);
            var count = 0;
            var fullName = typeof(M).FullName;
            foreach (var tp in tuples)
            {
                count++;
                if (count == 1)
                {
                    DC.Action = ActionEnum.Where;
                }
                else
                {
                    DC.Action = ActionEnum.And;
                }

                //
                if (tp.compare == CompareEnum.Like)
                {
                    DC.Option = OptionEnum.Like;
                    DC.Compare = CompareEnum.None;
                    DC.AddConditions(DC.DH.LikeDic(fullName, tp.key, string.Empty, tp.val, tp.valType));
                }
                else if (tp.compare == CompareEnum.In)
                {
                    DC.Option = OptionEnum.In;
                    DC.Compare = CompareEnum.None;
                    DC.AddConditions(DC.DH.InDic(fullName, tp.key, string.Empty, tp.val, tp.valType));
                }
                else if(tp.compare== CompareEnum.NotIn)
                {
                    DC.Option = OptionEnum.NotIn;
                    DC.Compare = CompareEnum.None;
                    DC.AddConditions(DC.DH.NotInDic(fullName, tp.key, string.Empty, tp.val, tp.valType));
                }
                else
                {
                    DC.Option = OptionEnum.Compare;
                    DC.Compare = tp.compare;
                    DC.AddConditions(DC.DH.CompareDic(fullName, tp.key, string.Empty, tp.val, tp.valType));
                }
            }
        }

        internal void AndHandle<M>(Expression<Func<M, bool>> func)
            where M : class
        {
            var field = DC.EH.FuncMBoolExpression(ActionEnum.And, func);
            field.Action = ActionEnum.And;
            DC.AddConditions(field);
        }

        internal void OrHandle<M>(Expression<Func<M, bool>> func)
            where M : class
        {
            var field = DC.EH.FuncMBoolExpression(ActionEnum.Or, func);
            field.Action = ActionEnum.Or;
            DC.AddConditions(field);
        }

        internal void OrderByHandle<M, F>(Expression<Func<M, F>> func, OrderByEnum orderBy)
            where M : class
        {
            var keyDic = DC.EH.FuncMFExpression(func)[0];
            switch (orderBy)
            {
                case OrderByEnum.Asc:
                    DC.Option = OptionEnum.Asc;
                    break;
                case OrderByEnum.Desc:
                    DC.Option = OptionEnum.Desc;
                    break;
            }

            DC.AddConditions(DC.DH.OrderbyDic(keyDic.ClassFullName, keyDic.ColumnOne,keyDic.TableAliasOne));
        }

        internal void OrderByHandle<F>(Expression<Func<F>> func, OrderByEnum orderBy)
        {
            var keyDic = DC.EH.FuncTExpression(func)[0];
            switch (orderBy)
            {
                case OrderByEnum.Asc:
                    DC.Option = OptionEnum.Asc;
                    break;
                case OrderByEnum.Desc:
                    DC.Option = OptionEnum.Desc;
                    break;
            }

            DC.AddConditions(DC.DH.OrderbyDic(keyDic.ClassFullName, keyDic.ColumnOne,keyDic.TableAliasOne));
        }

        protected void OrderByOptionHandle(PagingQueryOption option, string fullName)
        {
            if (option.OrderBys != null
              && option.OrderBys.Any())
            {
                foreach (var item in option.OrderBys)
                {
                    if (!string.IsNullOrWhiteSpace(item.Field))
                    {
                        DC.Action = ActionEnum.OrderBy;
                        if (item.Desc)
                        {
                            DC.Option = OptionEnum.Desc;
                        }
                        else
                        {
                            DC.Option = OptionEnum.Asc;
                        }
                        DC.Compare = CompareEnum.None;
                        DC.AddConditions(DC.DH.OrderbyDic(fullName, item.Field, string.Empty));
                    }
                }
            }
        }

        /****************************************************************************************************************************************/

    }
}
