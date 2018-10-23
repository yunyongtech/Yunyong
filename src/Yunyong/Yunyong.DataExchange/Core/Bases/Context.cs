using System.Collections.Generic;
using System.Data;
using System.Linq;
using Yunyong.DataExchange.AdoNet;
using Yunyong.DataExchange.Cache;
using Yunyong.DataExchange.Core.Common;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Core.Helper;
using Yunyong.DataExchange.Core.MySql;

namespace Yunyong.DataExchange.Core.Bases
{
    internal abstract class Context
    {

        internal void Init(IDbConnection conn)
        {
            Conn = conn;
            UiConditions = new List<DicModelUI>();
            DbConditions = new List<DicModelDB>();
            AH = new AttributeHelper(this);
            VH = new CsValueHelper(this);
            GH = GenericHelper.Instance;
            EH = new XExpression(this);
            SC = StaticCache.Instance;
            PH = new ParameterHelper(this);
            BDH = BatchDataHelper.Instance;
            SqlProvider = new MySqlProvider(this);
            DS = DataSource.Instance;
            DH = new DicModelHelper(this);
        }

        /************************************************************************************************************************/

        internal AttributeHelper AH { get; private set; }
        internal GenericHelper GH { get; private set; }
        internal ParameterHelper PH { get; private set; }
        internal BatchDataHelper BDH { get; private set; }

        /************************************************************************************************************************/

        internal XDebug Hint { get; set; }

        internal XExpression EH { get; private set; }
        internal CsValueHelper VH { get; private set; }
        internal DicModelHelper DH { get; private set; }

        /************************************************************************************************************************/

        internal CrudTypeEnum Crud { get; set; } = CrudTypeEnum.None;
        internal ActionEnum Action { get; set; } = ActionEnum.None;
        internal OptionEnum Option { get; set; } = OptionEnum.None;
        internal CompareEnum Compare { get; set; } = CompareEnum.None;

        /************************************************************************************************************************/

        internal List<DicModelUI> UiConditions { get; private set; }
        internal List<DicModelDB> DbConditions { get; private set; }

        /************************************************************************************************************************/

        internal IDbConnection Conn { get; private set; }
        internal IDbTransaction Tran { get; set; }

        /************************************************************************************************************************/

        internal MySqlProvider SqlProvider { get; set; }
        internal Operator OP { get; set; }
        internal Impler IP { get; set; }

        /************************************************************************************************************************/

        internal StaticCache SC { get; private set; }
        internal DataSource DS { get; private set; }

        /************************************************************************************************************************/

        internal bool IsParameter(DicModelUI item)
        {
            switch (item.Action)
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
        internal bool IsParameter(DicModelDB item)
        {
            switch (item.Action)
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

        internal void AddConditions(DicModelUI dic)
        {
            if (dic.CsValue!=null
                && (dic.Option == OptionEnum.In || dic.Option== OptionEnum.NotIn)
                && dic.CsValue.ToString().Contains(","))
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
                        else if(dic.Option ==  OptionEnum.NotIn)
                        {
                            op = OptionEnum.NotIn;
                        }
                    }
                    else
                    {
                        op = OptionEnum.InHelper;
                    }

                    //
                    var dicx = DicModelHelper.UiDicCopy(dic, val,dic.CsValueStr, op);
                    AddConditions(dicx);
                }
                UiConditions.Remove(dic);
            }
            else
            {
                //
                if(UiConditions.Count==0)
                {
                    dic.ID = 0;
                }
                else
                {
                    dic.ID = UiConditions.Max(it => it.ID) + 1;
                }

                //
                if(!string.IsNullOrWhiteSpace(dic.ParamRaw))
                {
                    dic.Param = $"{dic.ParamRaw}__{dic.ID}";
                }

                //
                UiConditions.Add(dic);
            }
        }

        internal void ResetConditions()
        {
            UiConditions = new List<DicModelUI>();
            DbConditions = new List<DicModelDB>();
        }

        internal void SetMTCache<M>()
        {
            //
            var type = typeof(M);
            var key = SC.GetKey(type.FullName, Conn.Database);

            //
            var table = SqlProvider.GetTableName<M>();
            SC.SetModelTableName(key, table);
            SC.SetModelType(key, type);
            SC.SetModelProperys(type, this);
            (SC.SetModelColumnInfos(key, this)).GetAwaiter().GetResult();
        }

    }
}
