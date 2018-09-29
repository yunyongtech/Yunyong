using System;
using System.Data;
using Yunyong.DataExchange.Common;
using Yunyong.DataExchange.Extensions;
using Yunyong.DataExchange.Others;

namespace Yunyong.DataExchange.Helper
{
    internal class ParameterPartHandle : ClassInstance<ParameterPartHandle>
    {
        private static ParamInfo GetDefault(string name, object value = null, DbType? dbType = null)
        {
            return new ParamInfo
            {
                Name = name,
                Value = value,
                ParameterDirection = ParameterDirection.Input,
                DbType = dbType,
                Size = null,
                Precision = null,
                Scale = null
            };
        }

        public ParamInfo BoolParamHandle(string colType, DicModelUI item)
        {
            if (!string.IsNullOrWhiteSpace(colType)
                && colType.Equals("bit", StringComparison.OrdinalIgnoreCase))
            {
                if (item.CsValue.ToBool())
                {
                    return GetDefault(item.Param, 1, DbType.UInt16);
                }
                else
                {
                    return GetDefault(item.Param, 0, DbType.UInt16);
                }
            }
            else
            {
                return GetDefault(item.Param, item.CsValue.ToBool(), DbType.Boolean);
            }
        }

        public ParamInfo EnumParamHandle(string colType, DicModelUI item)
        {
            if (!string.IsNullOrWhiteSpace(colType)
                && colType.Equals("int", StringComparison.OrdinalIgnoreCase))
            {
                var val = (int)(Enum.Parse(item.ValueType, item.CsValue, true));
                return GetDefault(item.Param, val, DbType.Int32);
            }
            else
            {
                return GetDefault(item.Param, item.CsValue.ToBool(), DbType.Boolean);
            }
        }

    }
}
