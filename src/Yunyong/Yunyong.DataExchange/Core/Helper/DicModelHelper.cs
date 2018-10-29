using System;
using System.Linq;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Core.Common;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Core.Extensions;

namespace Yunyong.DataExchange.Core.Helper
{
    internal class DicModelHelper
    {
        internal Context DC { get; set; }

        internal DicModelHelper(Context dc)
        {
            DC = dc;
        }

        /*******************************************************************************************************/

        internal static DicModelUI UiDicCopy(DicModelUI ui, object csVal, string csValStr, OptionEnum option)
        {
            var cp = new DicModelUI
            {
                //
                ID = ui.ID,
                Crud = ui.Crud,
                Action = ui.Action,
                Option = ui.Option,
                Compare = ui.Compare,
                Func= ui.Func,

                //
                ClassFullName = ui.ClassFullName,
                TableAliasOne = ui.TableAliasOne,
                ColumnOne = ui.ColumnOne,
                ColumnOneAlias = ui.ColumnOneAlias,
                TableTwo = ui.TableTwo,
                TableAliasTwo = ui.TableAliasTwo,
                ColumnTwo = ui.ColumnTwo,
                Param = ui.Param,
                ParamRaw = ui.ParamRaw,
                CsValue = ui.CsValue,
                CsValueStr = ui.CsValueStr,
                CsType = ui.CsType,
                TvpIndex = ui.TvpIndex
            };

            //
            cp.CsValue = csVal;
            cp.CsValueStr = csValStr;
            cp.Option = option;

            //
            return cp;
        }
        internal void UiToDbCopy()
        {
            if (DC.UiConditions != null)
            {
                foreach (var ui in DC.UiConditions)
                {
                    if (DC.DbConditions.Any(dm => dm.ID == ui.ID))
                    {
                        continue;
                    }

                    var db = new DicModelDB();

                    //
                    db.ID = ui.ID;
                    db.Crud = ui.Crud;
                    db.Action = ui.Action;
                    db.Option = ui.Option;
                    db.Compare = ui.Compare;
                    db.Func = ui.Func;

                    //
                    if (ui.ClassFullName.IsNullStr())
                    {
                        db.Key = string.Empty;
                    }
                    else
                    {
                        db.Key = DC.SC.GetModelKey(ui.ClassFullName);
                        db.TableOne = DC.SC.GetModelTableName(db.Key); 
                    }
                    db.TableAliasOne = ui.TableAliasOne;
                    db.ColumnOne = ui.ColumnOne;
                    db.KeyTwo = ui.ColumnTwo;
                    db.AliasTwo = ui.TableAliasTwo;
                    db.ColumnAlias = ui.ColumnOneAlias;
                    db.Param = ui.Param;
                    db.ParamRaw = ui.ParamRaw;
                    db.TvpIndex = ui.TvpIndex;
                    DC.PH.GetDbVal(ui, db, ui.CsType);
                    DC.DbConditions.Add(db);
                }
            }
        }

        internal void InNotInDicProcess(DicModelUI dic)
        {
            var vals = dic.CsValue.ToString().Split(',').Select(it => it);
            var i = 0;
            foreach (var val in vals)
            {
                //
                i++;
                var op = OptionEnum.None;
                if (i == 1)
                {
                    if (dic.Option == OptionEnum.In)
                    {
                        op = OptionEnum.In;
                    }
                    else if (dic.Option == OptionEnum.NotIn)
                    {
                        op = OptionEnum.NotIn;
                    }
                }
                else
                {
                    op = OptionEnum.InHelper;
                }

                //
                var dicx = DicModelHelper.UiDicCopy(dic, val, dic.CsValueStr, op);
                DC.AddConditions(dicx);
            }
            DC.UiConditions.Remove(dic);
        }
        internal void DicAddContext(DicModelUI dic)
        {
            //
            if (DC.UiConditions.Count == 0)
            {
                dic.ID = 0;
            }
            else
            {
                dic.ID = DC.UiConditions.Max(it => it.ID) + 1;
            }

            //
            if (!string.IsNullOrWhiteSpace(dic.ParamRaw))
            {
                dic.Param = $"{dic.ParamRaw}__{dic.ID}";
            }

            //
            DC.UiConditions.Add(dic);
        }
        
        private DicModelUI SetDicBase()
        {
            return new DicModelUI
            {
                ID = 0,
                Crud = DC.Crud,
                Action = DC.Action,
                Option = DC.Option,
                Compare = DC.Compare,
                Func= DC.Func
            };
        }

        /*******************************************************************************************************/

        internal DicModelUI CharLengthDic(string fullName,string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.Param = key;
            dic.ParamRaw = key;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;

            return dic;
        }

        internal DicModelUI TrimDic(string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.Param = key;
            dic.ParamRaw = key;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;

            return dic;
        }
        internal DicModelUI LTrimDic(string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.Param = key;
            dic.ParamRaw = key;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;

            return dic;
        }
        internal DicModelUI RTrimDic(string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.Param = key;
            dic.ParamRaw = key;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;

            return dic;
        }

        internal DicModelUI CompareDic(string classFullName, string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = classFullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;
            dic.Param = key;
            dic.ParamRaw = key;

            return dic;
        }

        internal DicModelUI InDic(string classFullName, string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = classFullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;
            dic.Param = key;
            dic.ParamRaw = key;

            return dic;
        }
        internal DicModelUI NotInDic(string classFullName, string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = classFullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;
            dic.Param = key;
            dic.ParamRaw = key;

            return dic;
        }

        internal DicModelUI LikeDic(string classFullName, string key, string alias, (object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = classFullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;
            dic.Param = key;
            dic.ParamRaw = key;

            return dic;
        }

        internal DicModelUI OneEqualOneDic((object val, string valStr) value, Type valType)
        {
            var dic = SetDicBase();
            dic.ColumnOne = "OneEqualOne";
            dic.Param = "OneEqualOne";
            dic.ParamRaw = "OneEqualOne";
            dic.CsValue = value.val;
            dic.CsValueStr = value.valStr;
            dic.CsType = valType;

            return dic;
        }

        internal DicModelUI IsNullDic(string classFullName, string key, string alias, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = classFullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;
            dic.Param = key;
            dic.ParamRaw = key;
            dic.CsValue = null;
            dic.CsValueStr = string.Empty;
            dic.CsType = valType;

            return dic;
        }

        /*******************************************************************************************************/

        internal DicModelUI SelectMemberInitDic(string fullName, string key, string alias, string colAlias)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.TableAliasOne = alias;
            dic.ColumnOne = key;
            dic.ColumnOneAlias = colAlias;

            return dic;
        }

        internal DicModelUI ColumnDic(string columnOne, string tableAliasOne, string fullName)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = columnOne;
            dic.TableAliasOne = tableAliasOne;

            return dic;
        }

        internal DicModelUI CountDic(string fullName, string key, string alias = "")
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.TableAliasOne = alias;
            dic.ColumnOne = key;
            dic.Param = key;
            dic.ParamRaw = key;
            
            return dic;
        }

        internal DicModelUI OrderbyDic(string fullName, string key,string alias)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = key;
            dic.TableAliasOne = alias;

            return dic;
        }

        /*******************************************************************************************************/

        internal DicModelUI InsertDic(string fullName, string key, (object val, string valStr) val, Type valType, int tvpIdx)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = key;
            dic.Param = key;
            dic.ParamRaw = key;
            dic.CsValue = val.val;
            dic.CsValueStr = val.valStr;
            dic.CsType = valType;
            dic.TvpIndex = tvpIdx;

            return dic;
        }

        /*******************************************************************************************************/

        internal DicModelUI SetDic(string fullName, string key, string param, (object val, string valStr) val, Type valType)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = key;
            dic.Param = param;
            dic.ParamRaw = param;
            dic.CsValue = val.val;
            dic.CsValueStr = val.valStr;
            dic.CsType = valType;
            
            return dic;
        }

        /*******************************************************************************************************/

        internal DicModelUI OnDic(string fullName, string key1, string alias1, string key2, string alias2)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = key1;
            dic.TableAliasOne = alias1;
            dic.ColumnTwo = key2;
            dic.TableAliasTwo = alias2;

            return dic;
        }

        /*******************************************************************************************************/

        internal DicModelUI TableDic(string fullName, string alias)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.TableAliasOne = alias;
            return dic;
        }

        internal DicModelUI ColumnDic(string fullName, string key)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.ColumnOne = key;

            return dic;
        }

        internal DicModelUI JoinColumnDic(string fullName, string key, string alias)
        {
            var dic = SetDicBase();
            dic.ClassFullName = fullName;
            dic.TableAliasOne = alias;
            dic.ColumnOne = key;
            return dic;
        }

    }
}
