using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Core.Common;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Core.Extensions;

namespace Yunyong.DataExchange.DataRainbow.MySQL
{
    internal sealed class MySqlProvider
        : MySql, ISqlProvider
    {
        private Context DC { get; set; }
        private StringBuilder X { get; set; } = new StringBuilder();

        private MySqlProvider() { }
        internal MySqlProvider(Context dc)
        {
            DC = dc;
            DC.SqlProvider = this;
        }

        /****************************************************************************************************************/

        private void OrderByParams()
        {
            var orders = DC.Parameters.Where(it => IsOrderByParam(it)).ToList();
            var i = 0;
            foreach (var o in orders)
            {
                i++;
                if (o.Func == FuncEnum.None)
                {
                    if (DC.Crud == CrudEnum.Join)
                    {
                        Column(o.TableAliasOne, o.ColumnOne, X); Spacing(X); Option(o.Option, X);
                    }
                    else
                    {
                        Column(string.Empty, o.ColumnOne, X); Spacing(X); Option(o.Option, X);
                    }
                }
                else if (o.Func == FuncEnum.CharLength)
                {
                    if (DC.Crud == CrudEnum.Join)
                    {
                        Function(o.Func, X); LeftBracket(X); Column(o.TableAliasOne, o.ColumnOne, X); RightBracket(X); Spacing(X); Option(o.Option, X);
                    }
                    else
                    {
                        Function(o.Func, X); LeftBracket(X); Column(string.Empty, o.ColumnOne, X); RightBracket(X); Spacing(X); Option(o.Option, X);
                    }
                }
                else
                {
                    throw new Exception($"{XConfig.EC._013} -- [[{o.Action}-{o.Option}-{o.Func}]] 不能解析!!!");
                }
                if (i != orders.Count) { Comma(X); }
            }
        }
        private void InParams(List<DicParam> dbs)
        {
            var i = 0;
            foreach (var it in dbs)
            {
                i++;
                DbParam(it.Param, X);
                if (i != dbs.Count) { Comma(X); }
            }
        }
        private void ConcatWithComma(IEnumerable<string> ss, Action<StringBuilder> preSymbol, Action<StringBuilder> afterSymbol)
        {
            var n = ss.Count();
            var i = 0;
            foreach (var s in ss)
            {
                i++;
                preSymbol?.Invoke(X); X.Append(s); afterSymbol?.Invoke(X);
                if (i != n)
                {
                    Comma(X);
                }
            }
        }
        private void LikeStrHandle(DicParam dic)
        {
            Spacing(X);
            var name = dic.Param;
            var value = dic.ParamInfo.Value.ToString();
            if (!value.Contains("%")
                && !value.Contains("_"))
            {
                X.Append("CONCAT");
                LeftBracket(X); SingleQuote(X); Percent(X); SingleQuote(X); Comma(X); DbParam(name, X); Comma(X); SingleQuote(X); Percent(X); SingleQuote(X); RightBracket(X);
            }
            else if ((value.Contains("%") || value.Contains("_"))
                && !value.Contains("/%")
                && !value.Contains("/_"))
            {
                DbParam(name, X);
            }
            else if (value.Contains("/%")
                || value.Contains("/_"))
            {
                DbParam(name, X); Spacing(X); Escape(X); Spacing(X); SingleQuote(X); EscapeChar(X); SingleQuote(X);
            }
            else
            {
                throw new Exception($"{XConfig.EC._015} -- [[{dic.Action}-{dic.Option}-{value}]] 不能解析!!!");
            }
        }

        /****************************************************************************************************************/

        private void CharLengthProcess(DicParam db)
        {
            Spacing(X);
            Function(db.Func, X); LeftBracket(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }
            RightBracket(X);
            Compare(db.Compare, X); DbParam(db.Param, X);
        }
        private void DateFormatProcess(DicParam db)
        {
            Spacing(X);
            Function(db.Func, X); LeftBracket(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }

            Comma(X); SingleQuote(X); X.Append(db.Format); SingleQuote(X);
            RightBracket(X); Compare(db.Compare, X); DbParam(db.Param, X);
        }
        private void TrimProcess(DicParam db)
        {
            Spacing(X);
            Function(db.Func, X); LeftBracket(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }
            RightBracket(X);
            Compare(db.Compare, X); DbParam(db.Param, X);
        }
        private void InProcess(DicParam db)
        {
            Spacing(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }
            Spacing(X);
            Function(db.Func, X); LeftBracket(X); InParams(db.InItems); RightBracket(X);
        }

        /****************************************************************************************************************/

        private void CompareProcess(DicParam db)
        {
            Spacing(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }
            Compare(db.Compare, X); DbParam(db.Param, X);
        }
        private void FunctionProcess(DicParam db)
        {
            if (db.Func == FuncEnum.CharLength)
            {
                CharLengthProcess(db);
            }
            else if (db.Func == FuncEnum.DateFormat)
            {
                DateFormatProcess(db);
            }
            else if (db.Func == FuncEnum.Trim || db.Func == FuncEnum.LTrim || db.Func == FuncEnum.RTrim)
            {
                TrimProcess(db);
            }
            else if (db.Func == FuncEnum.In || db.Func == FuncEnum.NotIn)
            {
                InProcess(db);
            }
            else
            {
                throw new Exception($"{XConfig.EC._006} -- [[{db.Func}]] 不能处理!!!");
            }
        }
        private void LikeProcess(DicParam db)
        {
            Spacing(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }
            Option(db.Option, X); LikeStrHandle(db);
        }
        private void OneEqualOneProcess(DicParam db)
        {
            Spacing(X); DbParam(db.Param, X);
        }
        private void IsNullProcess(DicParam db)
        {
            Spacing(X);
            if (db.Crud == CrudEnum.Join)
            {
                Column(db.TableAliasOne, db.ColumnOne, X);
            }
            else if (DC.IsSingleTableOption())
            {
                Column(string.Empty, db.ColumnOne, X);
            }
            Spacing(X); Option(db.Option, X);
        }

        /****************************************************************************************************************/

        private void MultiCondition(DicParam db)
        {
            if (db.Group != null)
            {
                var i = 0;
                foreach (var item in db.Group)
                {
                    i++;
                    if (item.Group != null)
                    {
                        LeftBracket(X); MultiCondition(item); RightBracket(X);
                    }
                    else
                    {
                        MultiCondition(item);
                    }
                    if (i != db.Group.Count)
                    {
                        MultiAction(db.GroupAction, X);
                    }
                }
            }
            else
            {
                if (db.Option == OptionEnum.Compare)
                {
                    CompareProcess(db);
                }
                else if (db.Option == OptionEnum.Function)
                {
                    FunctionProcess(db);
                }
                else if (db.Option == OptionEnum.Like)
                {
                    LikeProcess(db);
                }
                else if (db.Option == OptionEnum.OneEqualOne)
                {
                    OneEqualOneProcess(db);
                }
                else if (db.Option == OptionEnum.IsNull || db.Option == OptionEnum.IsNotNull)
                {
                    IsNullProcess(db);
                }
                else
                {
                    throw new Exception($"{XConfig.EC._011} -- [[{db.Action}-{db.Option}]] -- 不能解析!!!");
                }
            }
        }

        /****************************************************************************************************************/

        private void InsertColumn()
        {
            Spacing(X);
            var ps = DC.Parameters.FirstOrDefault(it => it.Action == ActionEnum.Insert && it.Option == OptionEnum.Insert);
            if (ps != null)
            {
                CRLF(X);
                LeftBracket(X); ConcatWithComma(ps.Inserts.Select(it => it.ColumnOne), Backquote, Backquote); RightBracket(X);
            }
        }
        private void UpdateColumn()
        {
            //
            var list = DC.Parameters.Where(it => it.Action == ActionEnum.Update)?.ToList();
            if (list == null || list.Count == 0) { throw new Exception("没有设置任何要更新的字段!"); }

            //
            if (DC.Set == SetEnum.AllowedNull)
            { }
            else if (DC.Set == SetEnum.NotAllowedNull)
            {
                if (list.Any(it => it.ParamInfo.Value == DBNull.Value))
                {
                    throw new Exception($"{DC.Set} -- 字段:[[{string.Join(",", list.Where(it => it.ParamInfo.Value == DBNull.Value).Select(it => it.ColumnOne))}]]的值不能设为 Null !!!");
                }
            }
            else if (DC.Set == SetEnum.IgnoreNull)
            {
                list = list.Where(it => it.ParamInfo.Value != DBNull.Value)?.ToList();
                if (list == null || list.Count == 0) { throw new Exception("没有设置任何要更新的字段!"); }
            }
            else
            {
                throw new Exception($"{XConfig.EC._012} -- [[{DC.Set}]] 不能解析!!!");
            }

            //
            Spacing(X);
            var i = 0;
            foreach (var item in list)
            {
                i++;
                if (item.Option == OptionEnum.ChangeAdd
                    || item.Option == OptionEnum.ChangeMinus)
                {
                    if (i != 1) { CRLF(X); Tab(X); }
                    Column(string.Empty, item.ColumnOne, X); Equal(X); Column(string.Empty, item.ColumnOne, X); Option(item.Option, X); DbParam(item.Param, X);
                }
                else if (item.Option == OptionEnum.Set)
                {
                    if (i != 1) { CRLF(X); Tab(X); }
                    Column(string.Empty, item.ColumnOne, X); Option(item.Option, X); DbParam(item.Param, X);
                }
                else
                {
                    throw new Exception($"{XConfig.EC._009} -- [[{item.Action}-{item.Option}]] 不能解析!!!");
                }
                if (i != list.Count) { Comma(X); }
            }
        }
        private void SelectColumn()
        {
            Spacing(X);
            var col = DC.Parameters.FirstOrDefault(it => IsSelectColumnParam(it));
            if (col == null)
            {
                Star(X);
                return;
            }
            var items = col.Columns.Where(it => it.Option == OptionEnum.Column || it.Option == OptionEnum.ColumnAs)?.ToList();
            if (items == null || items.Count <= 0)
            {
                Star(X);
                return;
            }
            var i = 0;
            foreach (var dic in items)
            {
                i++;
                if (i != 1) { CRLF(X); }
                if (items.Count > 1) { Tab(X); }
                if (dic.Func == FuncEnum.None)
                {
                    if (dic.Crud == CrudEnum.Join)
                    {
                        if (dic.Option == OptionEnum.Column)
                        {
                            Column(dic.TableAliasOne, dic.ColumnOne, X);
                        }
                        else if (dic.Option == OptionEnum.ColumnAs)
                        {
                            Column(dic.TableAliasOne, dic.ColumnOne, X); As(X); X.Append(dic.ColumnOneAlias);
                        }
                    }
                    else if (dic.Crud == CrudEnum.Query)
                    {
                        if (dic.Option == OptionEnum.Column)
                        {
                            Column(string.Empty, dic.ColumnOne, X);
                        }
                        else if (dic.Option == OptionEnum.ColumnAs)
                        {
                            Column(string.Empty, dic.ColumnOne, X); As(X); X.Append(dic.ColumnOneAlias);
                        }
                    }
                }
                else if (dic.Func == FuncEnum.DateFormat)
                {
                    if (dic.Crud == CrudEnum.Join)
                    {
                        if (dic.Option == OptionEnum.Column)
                        {
                            Function(dic.Func, X); LeftBracket(X); Column(dic.TableAliasOne, dic.ColumnOne, X); Comma(X); SingleQuote(X); X.Append(dic.Format); SingleQuote(X); RightBracket(X);
                        }
                        else if (dic.Option == OptionEnum.ColumnAs)
                        {
                            Function(dic.Func, X); LeftBracket(X); Column(dic.TableAliasOne, dic.ColumnOne, X); Comma(X); SingleQuote(X); X.Append(dic.Format); SingleQuote(X); RightBracket(X);
                            As(X); X.Append(dic.ColumnOneAlias);
                        }
                    }
                    else if (dic.Crud == CrudEnum.Query)
                    {
                        if (dic.Option == OptionEnum.Column)
                        {
                            Function(dic.Func, X); LeftBracket(X); Column(string.Empty, dic.ColumnOne, X); Comma(X); SingleQuote(X); X.Append(dic.Format); SingleQuote(X); RightBracket(X);
                        }
                        else if (dic.Option == OptionEnum.ColumnAs)
                        {
                            Function(dic.Func, X); LeftBracket(X); Column(string.Empty, dic.ColumnOne, X); Comma(X); SingleQuote(X); X.Append(dic.Format); SingleQuote(X); RightBracket(X);
                            As(X); X.Append(dic.ColumnOneAlias);
                        }
                    }
                }
                else
                {
                    throw new Exception($"{XConfig.EC._007} -- [[{dic.Func}]] 不能解析!!!");
                }
                if (i != items.Count) { Comma(X); }
            }
        }
        private void InsertValue()
        {
            Spacing(X);
            var items = DC.Parameters.Where(it => it.Action == ActionEnum.Insert && it.Option == OptionEnum.Insert)?.ToList();
            var i = 0;
            foreach (var dic in items)
            {
                i++;
                CRLF(X); LeftBracket(X); ConcatWithComma(dic.Inserts.Select(it => it.Param), At, null); RightBracket(X);
                if (i != items.Count)
                {
                    Comma(X);
                }
            }
        }

        private void Table()
        {
            Spacing(X);
            if (DC.Crud == CrudEnum.Join)
            {
                var dic = DC.Parameters.FirstOrDefault(it => it.Action == ActionEnum.From);
                TableX(dic.TableOne, X);
                As(X); X.Append(dic.TableAliasOne);
                Join();
            }
            else
            {
                TableX(DC.XC.GetTableName(DC.XC.GetModelKey(DC.SingleOpName)), X);
            }
        }
        private void Join()
        {
            Spacing(X);
            foreach (var item in DC.Parameters)
            {
                if (item.Crud != CrudEnum.Join) { continue; }
                switch (item.Action)
                {
                    case ActionEnum.From: break;    // 已处理 
                    case ActionEnum.InnerJoin:
                    case ActionEnum.LeftJoin:
                        CRLF(X); Tab(X); Action(item.Action, X); Spacing(X);
                        X.Append(item.TableOne); As(X); X.Append(item.TableAliasOne);
                        break;
                    case ActionEnum.On:
                        CRLF(X); Tab(X); Tab(X); Action(item.Action, X); Spacing(X);
                        Column(item.TableAliasOne, item.ColumnOne, X); Compare(item.Compare, X); Column(item.TableAliasTwo, item.ColumnTwo, X);
                        break;
                }
            }
        }
        private void Where()
        {
            var cons = DC.Parameters.Where(it => it.Action == ActionEnum.Where || it.Action == ActionEnum.And || it.Action == ActionEnum.Or)?.ToList();
            if (cons == null)
            {
                return;
            }
            var where = cons.FirstOrDefault(it => it.Action == ActionEnum.Where);
            var and = cons.FirstOrDefault(it => it.Action == ActionEnum.And);
            var or = cons.FirstOrDefault(it => it.Action == ActionEnum.Or);
            if (where == null
                && (and != null || or != null))
            {
                var aId = and == null ? -1 : and.ID;
                var oId = or == null ? -1 : or.ID;
                if (aId < oId
                    || oId == -1)
                {
                    Action(ActionEnum.Where, X); Spacing(X); X.Append("true"); Spacing(X);
                }
                else
                {
                    Action(ActionEnum.Where, X); Spacing(X); X.Append("false"); Spacing(X);
                }
            }
            foreach (var db in cons)
            {
                CRLF(X); Action(db.Action, X); Spacing(X);
                if (db.Group == null)
                {
                    MultiCondition(db);
                }
                else
                {
                    LeftBracket(X); MultiCondition(db); RightBracket(X);
                }
            }
        }
        private void OrderBy()
        {
            var dic = DC.Parameters.FirstOrDefault(it => it.Action == ActionEnum.From);
            var key = dic != null ? dic.Key : DC.XC.GetModelKey(DC.SingleOpName);
            var cols = DC.XC.GetColumnInfos(key);
            if (DC.Parameters.Any(it => it.Action == ActionEnum.OrderBy))
            {
                CRLF(X); X.Append("order by"); Spacing(X); OrderByParams();
            }
            else
            {
                if (!IsPaging(DC))
                {
                    return;
                }

                var col = GetIndex(cols);
                if (col != null)
                {
                    CRLF(X); X.Append("order by"); Spacing(X);
                    if (DC.Crud == CrudEnum.Join)
                    {
                        Column(dic.TableAliasOne, col.ColumnName, X); Spacing(X); X.Append("desc");
                    }
                    else
                    {
                        Column(string.Empty, col.ColumnName, X); Spacing(X); X.Append("desc");
                    }
                }
            }
        }
        private void Limit()
        {
            if (DC.PageIndex.HasValue
                && DC.PageSize.HasValue)
            {
                var start = default(int);
                if (DC.PageIndex > 0)
                {
                    start = ((DC.PageIndex - 1) * DC.PageSize).ToInt();
                }
                CRLF(X); X.Append("limit"); Spacing(X); X.Append(start); Comma(X); X.Append(DC.PageSize);
            }
        }

        private void Distinct()
        {
            if (DC.Parameters.Any(it => IsDistinctParam(it)))
            {
                Distinct(X);
            }
        }
        private void CountCD()
        {
            /* 
             * count(*)
             * count(col)
             * count(distinct col)
             */
            Spacing(X);
            var col = DC.Parameters.FirstOrDefault(it => IsSelectColumnParam(it));
            var item = col?.Columns?.FirstOrDefault(it => it.Option == OptionEnum.Column && it.Func == FuncEnum.Count);
            if (item != null)
            {
                if (item.Crud == CrudEnum.Query)
                {
                    if ("*".Equals(item.ColumnOne, StringComparison.OrdinalIgnoreCase))
                    {
                        Function(item.Func, X); LeftBracket(X); X.Append(item.ColumnOne); RightBracket(X);
                    }
                    else
                    {
                        Function(item.Func, X); LeftBracket(X); Distinct(); Column(string.Empty, item.ColumnOne, X); RightBracket(X);
                    }
                }
                else if (item.Crud == CrudEnum.Join)
                {
                    if ("*".Equals(item.ColumnOne, StringComparison.OrdinalIgnoreCase))
                    {
                        Function(item.Func, X); LeftBracket(X); X.Append(item.ColumnOne); RightBracket(X);
                    }
                    else
                    {
                        Function(item.Func, X); LeftBracket(X); Distinct(); Column(item.TableAliasOne, item.ColumnOne, X); RightBracket(X);
                    }
                }
            }
            else
            {
                X.Append("count(*)");
            }
        }
        private void Sum()
        {
            Spacing(X);
            var col = DC.Parameters.FirstOrDefault(it => IsSelectColumnParam(it));
            var item = col.Columns.FirstOrDefault(it => it.Func == FuncEnum.Sum);
            if (item.Crud == CrudEnum.Query)
            {
                Function(item.Func, X); LeftBracket(X); Column(string.Empty, item.ColumnOne, X); RightBracket(X);
            }
            else if (item.Crud == CrudEnum.Join)
            {
                Function(item.Func, X); LeftBracket(X); Column(item.TableAliasOne, item.ColumnOne, X); RightBracket(X);
            }
        }

        /****************************************************************************************************************/

        string ISqlProvider.GetTableName<M>()
        {
            var tableName = string.Empty;
            tableName = DC.AH.GetAttributePropVal<M, XTableAttribute>(a => a.Name);
            if (tableName.IsNullStr())
            {
                tableName = DC.AH.GetAttributePropVal<M, TableAttribute>(a => a.Name);
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new Exception($"类 [[{typeof(M).FullName}]] 必须是与 DB Table 对应的实体类,并且要由 [XTable] 或 [Table] 标记指定类对应的表名!!!");
            }
            return tableName;
        }
        async Task<List<ColumnInfo>> ISqlProvider.GetColumnsInfos(string tableName)
        {
            DC.SQL.Clear();
            DC.SQL.Add($@"
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
                                            WHERE  ( 
                                                                table_schema='{DC.Conn.Database.Trim().ToUpper()}' 
                                                                or table_schema='{DC.Conn.Database.Trim().ToLower()}' 
                                                                or table_schema='{DC.Conn.Database.Trim()}' 
                                                                or table_schema='{DC.Conn.Database}' 
                                                            )
                                                            and  ( 
                                                                        TABLE_NAME = '{tableName.Trim().ToUpper()}' 
                                                                        or TABLE_NAME = '{tableName.Trim().ToLower()}' 
                                                                        or TABLE_NAME = '{tableName.Trim()}' 
                                                                        or TABLE_NAME = '{tableName}' 
                                                                        )
                                            ;
                                  ");
            return await DC.DS.ExecuteReaderMultiRowAsync<ColumnInfo>();
        }
        void ISqlProvider.GetSQL()
        {
            DC.SQL.Clear();
            switch (DC.Method)
            {
                case UiMethodEnum.CreateAsync:
                case UiMethodEnum.CreateBatchAsync:
                    InsertInto(X); Table(); InsertColumn(); Values(X); InsertValue(); End(X, DC.SQL);
                    break;
                case UiMethodEnum.DeleteAsync:
                    Delete(X); From(X); Table(); Where(); End(X, DC.SQL);
                    break;
                case UiMethodEnum.UpdateAsync:
                    Update(X); Table(); Set(X); UpdateColumn(); Where(); End(X, DC.SQL);
                    break;
                case UiMethodEnum.TopAsync:
                case UiMethodEnum.ListAsync:
                case UiMethodEnum.AllAsync:
                case UiMethodEnum.FirstOrDefaultAsync:
                    Select(X); Distinct(); SelectColumn(); From(X); Table(); Where(); OrderBy(); Limit(); End(X, DC.SQL);
                    break;
                case UiMethodEnum.PagingListAsync:
                case UiMethodEnum.PagingAllAsync:
                    Select(X); CountCD(); From(X); Table(); Where(); End(X, DC.SQL);
                    Select(X); Distinct(); SelectColumn(); From(X); Table(); Where(); OrderBy(); Limit(); End(X, DC.SQL);
                    break;
                case UiMethodEnum.ExistAsync:
                case UiMethodEnum.CountAsync:
                    Select(X); CountCD(); From(X); Table(); Where(); End(X, DC.SQL);
                    break;
                case UiMethodEnum.SumAsync:
                    Select(X); Sum(); From(X); Table(); Where(); End(X, DC.SQL);
                    break;
            }
            if (XConfig.IsDebug)
            {
                XDebug.SQL = DC.SQL;
            }
        }
    }
}
