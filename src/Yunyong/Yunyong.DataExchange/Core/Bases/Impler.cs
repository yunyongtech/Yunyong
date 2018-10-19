using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.Core;
using Yunyong.DataExchange.Core.Common;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Core.Extensions;

namespace Yunyong.DataExchange.Core.Bases
{
    internal abstract class Impler
        : Operator
    {

        /**********************************************************************************************************/

        internal void ConvertDic()
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

                    //
                    if (ui.ClassFullName.IsNullStr())
                    {
                        db.Key = string.Empty;
                    }
                    else
                    {
                        db.Key = DC.SC.GetKey(ui.ClassFullName, DC.Conn.Database);
                        db.TableOne = DC.SC.GetModelTableName(db.Key); //ui.TableOne;
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

        /**********************************************************************************************************/

        private void SetInsertValue<M>(M m, int index)
        {
            var key = DC.SC.GetKey(m.GetType().FullName, DC.Conn.Database);
            var props = DC.SC.GetModelProperys(key);
            var columns = DC.SC.GetColumnInfos(key);
            var fullName = typeof(M).FullName;

            foreach (var prop in props)
            {
                var val = DC.VH.PropertyValue(prop, m);
                //DC.Option = option;
                DC.Compare = CompareEnum.None;
                DC.AddConditions(DC.DH.InsertDic(fullName, prop.Name, val, prop.PropertyType, index));
            }
        }

        /**********************************************************************************************************/

        protected async Task<PagingList<VM>> QueryPagingListAsyncHandle<M, VM>(int pageIndex, int pageSize, UiMethodEnum sqlType)
        {
            var result = new PagingList<VM>();
            result.PageIndex = pageIndex;
            result.PageSize = pageSize;
            var paras = DC.SqlProvider.GetParameters();
            var sql = DC.SqlProvider.GetSQL<M>(sqlType, result.PageIndex, result.PageSize);
            result.TotalCount = await DC.DS.ExecuteScalarAsync<int>(DC.Conn, sql[0], paras);
            result.Data = (await DC.DS.ExecuteReaderMultiRowAsync<VM>(DC.Conn, sql[1], paras)).ToList();
            return result;
        }

        /**********************************************************************************************************/

        internal void SelectMHandle<M>()
        {
            DC.Action = ActionEnum.Select;
            var vmType = typeof(M);
            var fullName = vmType.FullName;
            var vmProps = DC.GH.GetPropertyInfos(vmType);
            var tab = DC.UiConditions.FirstOrDefault(it => fullName.Equals(it.ClassFullName, StringComparison.OrdinalIgnoreCase));
            if (tab != null)
            {
                DC.Option = OptionEnum.Column;
                DC.Compare = CompareEnum.None;
                foreach (var prop in vmProps)
                {
                    DC.AddConditions(DC.DH.ColumnDic(prop.Name, tab.TableAliasOne, fullName));
                }
            }
            else if (DC.UiConditions.Count == 0)
            {
                // none
            }
            else
            {
                var fullNames = DC.UiConditions.Where(it => !string.IsNullOrWhiteSpace(it.ClassFullName)).Distinct();
                throw new Exception($"请使用 [[Task<List<VM>> QueryListAsync<VM>(Expression<Func<VM>> func)]] 方法! 或者 {vmType.Name} 必须为 [[{string.Join(",", fullNames.Select(it => it.ClassName))}]] 其中之一 !");
            }
        }

        internal void SelectMHandle<M, VM>()
        {
            var mType = typeof(M);
            var vmType = typeof(VM);
            if (mType == vmType)
            {
                return;
            }

            //
            DC.Action = ActionEnum.Select;
            var fullName = mType.FullName;
            var mProps = DC.GH.GetPropertyInfos(mType);
            var tab = DC.UiConditions.FirstOrDefault(it => fullName.Equals(it.ClassFullName, StringComparison.OrdinalIgnoreCase));
            var vmProps = DC.GH.GetPropertyInfos(vmType);
            if (tab != null)
            {
                DC.Option = OptionEnum.Column;
                DC.Compare = CompareEnum.None;
                foreach (var prop in mProps)
                {
                    foreach (var vProp in vmProps)
                    {
                        if (prop.Name.Equals(vProp.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            DC.AddConditions(DC.DH.ColumnDic(prop.Name, tab.TableAliasOne, fullName));
                        }
                    }
                }
            }
            else if (DC.UiConditions.Count == 0)
            {
                // none
            }
            else
            {
                var fullNames = DC.UiConditions.Where(it => !string.IsNullOrWhiteSpace(it.ClassFullName)).Distinct();
                throw new Exception($"请使用 [[Task<List<VM>> QueryListAsync<VM>(Expression<Func<VM>> func)]] 方法! 或者 {mType.Name} 必须为 [[{string.Join(",", fullNames.Select(it => it.ClassName))}]] 其中之一 !");
            }
        }

        internal void SelectMHandle<VM>(Expression<Func<VM>> func)
        {
            DC.Action = ActionEnum.Select;
            var list = DC.EH.FuncMExpression(func);
            foreach (var dic in list)
            {
                dic.Option = OptionEnum.ColumnAs;
                DC.AddConditions(dic);
            }
        }

        internal void SelectMHandle<M, VM>(Expression<Func<M, VM>> func)
            where M : class
        {
            DC.Action = ActionEnum.Select;
            var list = DC.EH.FuncMFExpression(func);
            foreach (var dic in list)
            {
                dic.Option = OptionEnum.ColumnAs;
                DC.AddConditions(dic);
            }
        }

        /**********************************************************************************************************/

        internal void CreateMHandle<M>(M m)
        {
            DC.Option = OptionEnum.Insert;
            SetInsertValue(m, 0);
        }
        internal void CreateMHandle<M>(IEnumerable<M> mList)
        {
            var i = 0;
            DC.Option = OptionEnum.InsertTVP;
            foreach (var m in mList)
            {
                SetInsertValue(m, i);
                i++;
            }
        }

        /**********************************************************************************************************/

        internal Impler(Context dc)
            : base(dc)
        {
            DC.IP = this;
            ConvertDic();
        }
    }
}
