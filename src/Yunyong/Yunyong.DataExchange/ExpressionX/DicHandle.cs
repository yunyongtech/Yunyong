using System;
using System.Linq.Expressions;
using Yunyong.DataExchange.Common;
using Yunyong.DataExchange.Enums;

namespace Yunyong.DataExchange.ExpressionX
{
    internal static class DicHandle
    {

        // 02
        internal static CompareEnum GetOption(ExpressionType nodeType, bool isR)
        {
            var option = CompareEnum.None;
            if (nodeType == ExpressionType.Equal)
            {
                option = !isR ? CompareEnum.Equal : CompareEnum.Equal;
            }
            else if (nodeType == ExpressionType.NotEqual)
            {
                option = !isR ? CompareEnum.NotEqual : CompareEnum.NotEqual;
            }
            else if (nodeType == ExpressionType.LessThan)
            {
                option = !isR ? CompareEnum.LessThan : CompareEnum.GreaterThan;
            }
            else if (nodeType == ExpressionType.LessThanOrEqual)
            {
                option = !isR ? CompareEnum.LessThanOrEqual : CompareEnum.GreaterThanOrEqual;
            }
            else if (nodeType == ExpressionType.GreaterThan)
            {
                option = !isR ? CompareEnum.GreaterThan : CompareEnum.LessThan;
            }
            else if (nodeType == ExpressionType.GreaterThanOrEqual)
            {
                option = !isR ? CompareEnum.GreaterThanOrEqual : CompareEnum.LessThanOrEqual;
            }

            return option;
        }

        /*******************************************************************************************************/

        internal static DicModel BinaryCharLengthHandle(string key, string alias, string value, Type valType, ExpressionType nodeType, bool isR)
        {
            return new DicModel
            {
                ColumnOne = key,
                TableAliasOne = alias,
                Param = key,
                ParamRaw = key,
                CsValue = value,
                ValueType = valType,
                Option = OptionEnum.CharLength,
                Compare = GetOption(nodeType, isR)
            };
        }
        // 01
        internal static DicModel BinaryNormalHandle(string key, string alias, string value, Type valType, ExpressionType nodeType, bool isR)
        {
            return new DicModel
            {
                ColumnOne = key,
                TableAliasOne = alias,
                CsValue = value,
                ValueType = valType,
                Param = key,
                ParamRaw = key,
                Option = OptionEnum.Compare,
                Compare = GetOption(nodeType, isR)
            };
        }
        // 01
        internal static DicModel CallInHandle(string key, string alias, string value, Type valType)
        {
            //if (valType.IsEnum)
            //{
            //    valType = typeof(int);
            //}
            return new DicModel
            {
                ColumnOne = key,
                TableAliasOne = alias,
                Param = key,
                ParamRaw = key,
                CsValue = value,
                ValueType = valType,
                Option = OptionEnum.In
            };
        }
        // 01
        internal static DicModel CallLikeHandle(string key, string alias, string value, Type valType)
        {
            return new DicModel
            {
                ColumnOne = key,
                TableAliasOne = alias,
                Param = key,
                ParamRaw = key,
                CsValue = value,
                ValueType = valType,
                Option = OptionEnum.Like
            };
        }
        // 01
        internal static DicModel ConstantBoolHandle(string value, Type valType)
        {
            return new DicModel
            {
                ColumnOne = "OneEqualOne",
                Param = "OneEqualOne",
                ParamRaw = "OneEqualOne",
                CsValue = value,
                ValueType = valType,
                Option = OptionEnum.OneEqualOne
            };
        }
        // 01
        internal static DicModel MemberBoolHandle(string key, string alias, Type valType)
        {
            return new DicModel
            {
                ColumnOne = key,
                TableAliasOne = alias,
                Param = key,
                ParamRaw = key,
                CsValue = true.ToString(),
                ValueType = valType,
                Option = OptionEnum.Compare,
                Compare = CompareEnum.Equal
            };
        }

        /*******************************************************************************************************/

        internal static DicModel SelectColumnHandle(string columnOne, string tableAliasOne)
        {
            return new DicModel
            {
                ColumnOne = columnOne,
                TableAliasOne = tableAliasOne,
                Action = ActionEnum.Select,
                Option = OptionEnum.Column,
                Crud = CrudTypeEnum.Join
            };
        }

        /*******************************************************************************************************/

        internal static DicModel ConditionCountHandle(string key)
        {
            return new DicModel
            {
                ColumnOne = key,
                Param = key,
                ParamRaw = key,
                Action = ActionEnum.Select,
                Option = OptionEnum.Count,
                Crud = CrudTypeEnum.Query
            };
        }

    }
}
