using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.DataExchange.Core;
using Yunyong.DataExchange.Enums;
using Yunyong.DataExchange.Helper;

namespace Yunyong.DataExchange.UserFacade.Join
{
    public class QueryFilter : Operator
    {

        internal QueryFilter(DbContext dc)
        {
            DC = dc;
        }

        public QueryFilter And(Expression<Func<bool>> func)
        {
            var field = DC.EH.ExpressionHandle(func, ActionEnum.And);
            field.Crud = CrudTypeEnum.Join;
            DC.AddConditions(field);
            return this;
        }

        public async Task<List<VM>> QueryListAsync<VM>()
        {
            return (await SqlHelper.QueryAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<VM>(SqlTypeEnum.JoinQueryListAsync)[0],
                DC.GetParameters())).ToList();
        }

    }
}
