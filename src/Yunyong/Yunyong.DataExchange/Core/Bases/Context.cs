using System;
using System.Collections.Generic;
using System.Data;
using Yunyong.DataExchange.AdoNet;
using Yunyong.DataExchange.Core.Common;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Core.Helper;
using Yunyong.DataExchange.DataRainbow.MySQL;

namespace Yunyong.DataExchange.Core.Bases
{
    internal abstract class Context
    {

        /************************************************************************************************************************/

        internal void Init(IDbConnection conn)
        {
            //
            if (XConfig.DB == DbEnum.None)
            {
                if (XConfig.MySQL.Equals(conn.GetType().FullName, StringComparison.OrdinalIgnoreCase))
                {
                    XConfig.DB = DbEnum.MySQL;
                }
                else
                {
                    throw new Exception("MyDAL 目前只支持 【MySQL】,后续将会支持【Oracle/SQLServer/PostgreSQL/DB2/Access/SQLite/Teradata/MariaDB】.");
                }
            }

            //
            Conn = conn;
            Parameters = new List<DicParam>();
            AH = new AttributeHelper(this);
            VH = new CsValueHelper(this);
            GH = new GenericHelper(this);
            EH = new XExpression(this);
            SC = new XCache(this);
            PH = new ParameterHelper(this);
            DPH = new DicParamHelper(this);
            BDH = new BatchDataHelper();
            DS = new DataSource(this);

            //
            if (XConfig.DB == DbEnum.MySQL)
            {
                SqlProvider = new MySqlProvider(this);
            }
        }

        /************************************************************************************************************************/

        internal AttributeHelper AH { get; private set; }
        internal GenericHelper GH { get; private set; }
        internal ParameterHelper PH { get; private set; }
        internal BatchDataHelper BDH { get; private set; }

        /************************************************************************************************************************/

        internal XExpression EH { get; private set; }
        internal CsValueHelper VH { get; private set; }
        internal DicParamHelper DPH { get; private set; }

        /************************************************************************************************************************/

        internal CrudTypeEnum Crud { get; set; } = CrudTypeEnum.None;
        internal ActionEnum Action { get; set; } = ActionEnum.None;
        internal OptionEnum Option { get; set; } = OptionEnum.None;
        internal CompareEnum Compare { get; set; } = CompareEnum.None;
        internal FuncEnum Func { get; set; } = FuncEnum.None;
        internal UiMethodEnum Method { get; set; } = UiMethodEnum.None;

        /************************************************************************************************************************/

        internal bool NeedSetSingle { get; set; } = true;
        internal string SingleOpName { get; set; }
        internal int DicID { get; set; } = 1;
        internal List<DicParam> Parameters { get; set; }
        internal List<string> SQL { get; set; }
        internal int? PageIndex { get; set; } = null;
        internal int? PageSize { get; set; } = null;

        /************************************************************************************************************************/

        internal IDbConnection Conn { get; private set; }
        internal IDbTransaction Tran { get; set; }

        /************************************************************************************************************************/

        internal ISqlProvider SqlProvider { get; set; }
        internal Operator OP { get; set; }
        internal Impler IP { get; set; }

        /************************************************************************************************************************/

        internal XCache SC { get; private set; }
        internal DataSource DS { get; private set; }

        /************************************************************************************************************************/

        internal bool IsInParameter(object value, OptionEnum option)
        {
            if (value != null
                && (option == OptionEnum.In || option == OptionEnum.NotIn))
            {
                return true;
            }
            return false;
        }
        internal bool IsParameter(ActionEnum action)
        {
            switch (action)
            {
                case ActionEnum.Insert:
                case ActionEnum.Update:
                case ActionEnum.Where:
                case ActionEnum.And:
                case ActionEnum.Or:
                    return true;
            }
            return false;
        }
        internal bool IsFilterCondition(ActionEnum action)
        {
            switch (action)
            {
                case ActionEnum.Where:
                case ActionEnum.And:
                case ActionEnum.Or:
                    return true;
            }
            return false;
        }
        internal bool IsSingleTableOption()
        {
            switch (Crud)
            {
                case CrudTypeEnum.Query:
                case CrudTypeEnum.Update:
                case CrudTypeEnum.Delete:
                case CrudTypeEnum.Create:
                    return true;
            }
            return false;
        }

        /************************************************************************************************************************/

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

        internal void SetMTCache<M>()
        {
            //
            var type = typeof(M);
            var key = SC.GetModelKey(type.FullName);
            if (NeedSetSingle)
            {
                SingleOpName = type.FullName;
                NeedSetSingle = false;
            }

            //
            var table = SqlProvider.GetTableName<M>();
            SC.SetModelTableName(key, table);
            SC.SetModelType(key, type);
            SC.SetModelProperys(type, this);
            (SC.SetModelColumnInfos(key, this)).GetAwaiter().GetResult();
        }

    }
}
