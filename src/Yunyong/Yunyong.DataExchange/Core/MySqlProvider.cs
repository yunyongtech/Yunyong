﻿
using Yunyong.DataExchange.AdoNet;
using Yunyong.DataExchange.Common;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;
using Yunyong.DataExchange.Extensions;
using Yunyong.DataExchange.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Yunyong.DataExchange.Core
{
    internal class MySqlProvider
    {
        private DbContext DC { get; set; }

        private MySqlProvider() { }
        internal MySqlProvider(DbContext dc)
        {
            DC = dc;
            DC.SqlProvider = this;
        }

        /****************************************************************************************************************/

        private string LikeStrHandle(DicModel dic)
        {
            var name = dic.Param;
            var value = dic.Value;
            if (!value.Contains("%")
                && !value.Contains("_"))
            {
                return $"CONCAT('%',@{name},'%')";
            }
            else if ((value.Contains("%") || value.Contains("_"))
                && !value.Contains("/%")
                && !value.Contains("/_"))
            {
                return $"@{name}";
            }
            else if (value.Contains("/%")
                || value.Contains("/_"))
            {
                return $"@{name} escape '/' ";
            }

            throw new Exception(value);
        }
        private string InStrHandle(DicModel dic)
        {
            var paras = DC.Conditions.Where(it =>
            {
                if (!string.IsNullOrWhiteSpace(it.ParamRaw))
                {
                    return it.ParamRaw.Equals(dic.ParamRaw, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    return false;
                }
            }).ToList();
            return $" {string.Join(",", paras.Select(it => $" @{it.Param} "))} ";
        }

        private string GetOrderByPart<M>()
        {
            var str = string.Empty;
            var cols = (DC.SC.GetColumnInfos<M>(DC)).GetAwaiter().GetResult();

            if (DC.Conditions.Any(it => it.Action == ActionEnum.OrderBy))
            {
                str = string.Join(",", DC.Conditions.Where(it => it.Action == ActionEnum.OrderBy).Select(it => $" `{it.KeyOne}` {it.Option.ToEnumDesc<OptionEnum>()} "));
            }
            else if (cols.Any(it=> "PRI".Equals(it.KeyType,StringComparison.OrdinalIgnoreCase)))
            {
                str = string.Join(",", cols.Where(it => "PRI".Equals(it.KeyType, StringComparison.OrdinalIgnoreCase)).Select(it => $" `{it.ColumnName}` desc "));
            }
            else
            {
                str = $" `{DC.SC.GetModelProperys(typeof(M), DC).First().Name}` desc ";
            }

            return str;
        }

        internal string GetSingleValuePart()
        {
            var str = string.Empty;

            foreach (var item in DC.Conditions)
            {
                switch (item.Option)
                {
                    case OptionEnum.Count:
                        str += $" {item.Option.ToEnumDesc<OptionEnum>()}(`{item.KeyOne}`) ";
                        break;
                }
            }

            return str;
        }

        internal string Columns()
        {
            var str = string.Empty;
            var list = new List<string>();
            foreach (var dic in DC.Conditions)
            {
                if (dic.Option != OptionEnum.Column)
                {
                    continue;
                }

                switch (dic.Action)
                {
                    case ActionEnum.Select:
                        switch (dic.Crud)
                        {
                            case CrudTypeEnum.Join:
                                list.Add( $" \t {dic.AliasOne}.`{dic.KeyOne}` ");
                                break;
                        }
                        break;
                }
            }

            str = string.Join(",\r\n", list);
            return str;
        }

        private string From()
        {
            return "\r\n from";
        }

        private string Table<M>(UiMethodEnum type)
        {

            var tableName = string.Empty;
            if (type != UiMethodEnum.JoinQueryListAsync)
            {
                TryGetTableName<M>(out tableName);
            }

            return tableName;
        }

        internal string Joins()
        {
            var str = string.Empty;

            foreach (var item in DC.Conditions)
            {
                if (item.Crud != CrudTypeEnum.Join)
                {
                    continue;
                }

                switch (item.Action)
                {
                    case ActionEnum.From:
                        str += $" {item.TableOne} as {item.AliasOne} ";
                        break;
                    case ActionEnum.InnerJoin:
                    case ActionEnum.LeftJoin:
                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.TableOne} as {item.AliasOne} ";
                        break;
                    case ActionEnum.On:
                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.AliasOne}.`{item.KeyOne}`={item.AliasTwo}.`{item.KeyTwo}` ";
                        break;
                }
            }

            return str;
        }

        internal string Wheres()
        {
            //if (!DC.Conditions.Any(it => it.Action == ActionEnum.Where)
            //    && !DC.Conditions.Any(it => it.Action == ActionEnum.And)
            //    && !DC.Conditions.Any(it => it.Action == ActionEnum.Or))
            //{
            //    throw new Exception("没有设置任何条件!");
            //}

            var str = string.Empty;

            foreach (var item in DC.Conditions)
            {
                switch (item.Action)
                {
                    case ActionEnum.Where:
                    case ActionEnum.And:
                    case ActionEnum.Or:
                        switch (item.Option)
                        {
                            case OptionEnum.Equal:
                            case OptionEnum.NotEqual:
                            case OptionEnum.LessThan:
                            case OptionEnum.LessThanOrEqual:
                            case OptionEnum.GreaterThan:
                            case OptionEnum.GreaterThanOrEqual:
                                switch (item.Crud)
                                {
                                    case CrudTypeEnum.Join:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.AliasOne}.`{item.KeyOne}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.Param} ";
                                        break;
                                    case CrudTypeEnum.Delete:
                                    case CrudTypeEnum.Update:
                                    case CrudTypeEnum.Query:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} `{item.KeyOne}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.Param} ";
                                        break;
                                }
                                break;
                            case OptionEnum.Like:
                                switch (item.Crud)
                                {
                                    case CrudTypeEnum.Join:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.AliasOne}.`{item.KeyOne}`{item.Option.ToEnumDesc<OptionEnum>()}{LikeStrHandle(item)} ";
                                        break;
                                    case CrudTypeEnum.Delete:
                                    case CrudTypeEnum.Update:
                                    case CrudTypeEnum.Query:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} `{item.KeyOne}`{item.Option.ToEnumDesc<OptionEnum>()}{LikeStrHandle(item)} ";
                                        break;
                                }
                                break;
                            case OptionEnum.CharLength:
                                switch (item.Crud)
                                {
                                    case CrudTypeEnum.Join:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.Option.ToEnumDesc<OptionEnum>()}({item.AliasOne}.`{item.KeyOne}`){item.FuncSupplement.ToEnumDesc<OptionEnum>()}@{item.Param} ";
                                        break;
                                    case CrudTypeEnum.Delete:
                                    case CrudTypeEnum.Update:
                                    case CrudTypeEnum.Query:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.Option.ToEnumDesc<OptionEnum>()}(`{item.KeyOne}`){item.FuncSupplement.ToEnumDesc<OptionEnum>()}@{item.Param} ";
                                        break;
                                }
                                break;
                            case OptionEnum.OneEqualOne:
                                str += $" {item.Action.ToEnumDesc<ActionEnum>()} @{item.Param} ";
                                break;
                            case OptionEnum.In:
                                switch (item.Crud)
                                {
                                    case CrudTypeEnum.Join:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} {item.AliasOne}.`{item.KeyOne}` {item.Option.ToEnumDesc<OptionEnum>()}({InStrHandle(item)}) ";
                                        break;
                                    case CrudTypeEnum.Delete:
                                    case CrudTypeEnum.Update:
                                    case CrudTypeEnum.Query:
                                        str += $" {item.Action.ToEnumDesc<ActionEnum>()} `{item.KeyOne}` {item.Option.ToEnumDesc<OptionEnum>()}({InStrHandle(item)}) ";
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }

            str = str.Replace("where", "\r\n where");

            return str;
        }

        internal string GetUpdates()
        {
            if (//!DC.Conditions.Any(it => it.Action == ActionEnum.Set)
            //    && !DC.Conditions.Any(it => it.Action == ActionEnum.Change)
                !DC.Conditions.Any(it => it.Action == ActionEnum.Update))
            {
                throw new Exception("没有设置任何要更新的字段!");
            }

            var list = new List<string>();

            foreach (var item in DC.Conditions)
            {
                switch (item.Action)
                {
                    //case ActionEnum.Set:
                    //case ActionEnum.Change:
                    case ActionEnum.Update:
                        switch (item.Option)
                        {
                            case OptionEnum.ChangeAdd:
                            case OptionEnum.ChangeMinus:
                                list.Add($" `{item.KeyOne}`=`{item.KeyOne}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.Param} ");
                                break;
                            case OptionEnum.Set:
                                list.Add($" `{item.KeyOne}`{item.Option.ToEnumDesc<OptionEnum>()}@{item.Param} ");
                                break;
                        }
                        break;
                }
            }

            return string.Join(",", list);
        }

        internal async Task<List<ColumnInfo>> GetColumnsInfos<M>()
        {
            TryGetTableName<M>(out var tableName);
            var sql = $@"
                                        SELECT distinct
                                            TABLE_NAME as TableName,
                                            column_name as ColumnName,
                                            DATA_TYPE as DataType,
                                            column_default as ColumnDefault,
                                            is_nullable AS IsNullable,
                                            column_comment as ColumnComment,
                                            column_key as KeyType
                                        FROM
                                            information_schema.COLUMNS
                                        WHERE  table_schema='{DC.Conn.Database}'
                                            and  TABLE_NAME = '{tableName.TrimStart('`').TrimEnd('`').ToLower()}'
                                        ;
                                  ";
            return (await SqlHelper.QueryAsync<ColumnInfo>(DC.Conn, sql, new DynamicParameters())).ToList();
        }

        internal string GetColumns()
        {
            var list = new List<string>();
            foreach (var item in DC.Conditions)
            {
                if (item.TvpIndex == 0)
                {
                    list.Add($"`{item.KeyOne}`");
                }
            }
            return $" ({ string.Join(",", list)}) ";
        }
        internal string GetValues()
        {
            var list = new List<string>();
            for (var i = 0; i < DC.Conditions.Max(it => it.TvpIndex) + 1; i++)
            {
                var values = new List<string>();
                foreach (var item in DC.Conditions)
                {
                    if (item.TvpIndex == i)
                    {
                        values.Add($"@{item.Param}");
                    }
                }
                list.Add($"({string.Join(",", values)})");
            }
            return string.Join(",", list);
        }

        internal string GetTableName<M>(M m)
        {
            return $"`{DC.TableAttributeName(m.GetType())}`";
        }
        internal string GetTableName(Type mType)
        {
            return $"`{DC.TableAttributeName(mType)}`";
        }
        internal bool TryGetTableName<M>(out string tableName)
        {
            tableName = DC.AH.GetPropertyValue<TableAttribute>(typeof(M), a => a.Name);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception("DB Entity 缺少 TableAttribute 指定的表名!");
            }
            tableName = $"`{tableName}`";
            return true;
        }

        internal OptionEnum GetChangeOption(ChangeEnum change)
        {
            switch (change)
            {
                case ChangeEnum.Add:
                    return OptionEnum.ChangeAdd;
                case ChangeEnum.Minus:
                    return OptionEnum.ChangeMinus;
                default:
                    return OptionEnum.ChangeAdd;
            }
        }

        internal List<string> GetSQL<M>(UiMethodEnum type, int? pageIndex = null, int? pageSize = null)
        {
            var list = new List<string>();

            //
            switch (type)
            {
                case UiMethodEnum.CreateAsync:
                    list.Add($" insert into {Table<M>(type)} {GetColumns()} values {GetValues()} ;");
                    break;
                case UiMethodEnum.CreateBatchAsync:
                    list.Add(
                                  $" LOCK TABLES {Table<M>(type)} WRITE; " +
                                  $" /*!40000 ALTER TABLE {Table<M>(type)} DISABLE KEYS */; " +
                                  $" insert into  {Table<M>(type)} {GetColumns()} VALUES {GetValues()} ; " +
                                  $" /*!40000 ALTER TABLE {Table<M>(type)} ENABLE KEYS */; " +
                                  $" UNLOCK TABLES; "
                                 );
                    break;
                case UiMethodEnum.DeleteAsync:
                    list.Add($" delete {From()} {Table<M>(type)} {Wheres()} ; ");
                    break;
                case UiMethodEnum.UpdateAsync:
                    list.Add($" update {Table<M>(type)} set {DC.SqlProvider.GetUpdates()} {Wheres()} ;");
                    break;
                case UiMethodEnum.QueryFirstOrDefaultAsync:
                    list.Add($"select * {From()} {Table<M>(type)} {Wheres()} ; ");
                    break;
                case UiMethodEnum.QueryListAsync:
                    list.Add($"select * {From()} {Table<M>(type)} {Wheres()} ; ");
                    break;
                case UiMethodEnum.QueryPagingListAsync:
                    var wherePart = Wheres();
                    list.Add($"select count(*) {From()} {Table<M>(type)} {wherePart} ; ");
                    list.Add($"select * {From()} {Table<M>(type)} {wherePart} order by {GetOrderByPart<M>()} limit {(pageIndex - 1) * pageSize},{pageIndex * pageSize}  ; ");
                    break;
                case UiMethodEnum.QueryAllPagingListAsync:
                    list.Add($"select count(*) {From()} {Table<M>(type)} ; ");
                    list.Add($"select * {From()} {Table<M>(type)} order by {GetOrderByPart<M>()} limit {(pageIndex - 1) * pageSize},{pageIndex * pageSize}  ; ");
                    break;
                case UiMethodEnum.JoinQueryListAsync:
                    list.Add($" select {Columns()} {From()} {Joins()} {Wheres()} ; ");
                    break;
                case UiMethodEnum.QuerySingleValueAsync:
                    list.Add($" select {GetSingleValuePart()} {From()} {Table<M>(type)} {Wheres()} ; ");
                    break;
                case UiMethodEnum.ExistAsync:
                    list.Add($" select count(*) {From()} {Table<M>(type)} {Wheres()} ; ");
                    break;
                case UiMethodEnum.QueryAllAsync:
                    list.Add($" select * {From()} {Table<M>(type)} ; ");
                    break;
            }

            //
            if (XDebug.Hint)
            {
                XDebug.SQL = list;
                var paras = DC.GetParameters();
                XDebug.Parameters = DC
                    .Conditions
                    .Where(it => DC.IsParameter(it))
                    .Select(it =>
                    {
                        return $"key:【{it.Param}】;val:【{it.Value}】;param【{it.DbValue}】.";
                    })
                    .ToList();
            }

            //
            return list;
        }
    }
}
