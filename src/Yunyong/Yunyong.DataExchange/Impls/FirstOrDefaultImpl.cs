using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.Impls
{
    internal class FirstOrDefaultImpl<M>
        : Impler, IFirstOrDefault<M>
        where M : class
    {
        internal FirstOrDefaultImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<M> FirstOrDefaultAsync()
        {
            DC.Method = UiMethodEnum.FirstOrDefaultAsync;
            DC.SqlProvider.GetSQL();
            return await DC.DS.ExecuteReaderSingleRowAsync<M>();
        }

        public async Task<VM> FirstOrDefaultAsync<VM>()
            where VM:class
        {
            SelectMHandle<M, VM>();
            DC.DPH.SetParameter();
            DC.Method = UiMethodEnum.FirstOrDefaultAsync;
            DC.SqlProvider.GetSQL();
            return await DC.DS.ExecuteReaderSingleRowAsync<VM>();
        }

        public async Task<VM> FirstOrDefaultAsync<VM>(Expression<Func<M, VM>> func)
            where VM:class
        {
            SelectMHandle(func);
            DC.DPH.SetParameter();
            DC.Method = UiMethodEnum.FirstOrDefaultAsync;
            DC.SqlProvider.GetSQL();
            return await DC.DS.ExecuteReaderSingleRowAsync<VM>();
        }
    }

    internal class FirstOrDefaultXImpl
        : Impler, IFirstOrDefaultX
    {
        internal FirstOrDefaultXImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<M> FirstOrDefaultAsync<M>()
            where M:class
        {
            SelectMHandle<M>();
            DC.DPH.SetParameter();
            DC.Method = UiMethodEnum.JoinFirstOrDefaultAsync;
            DC.SqlProvider.GetSQL();
            return await DC.DS.ExecuteReaderSingleRowAsync<M>();
        }

        public async Task<VM> FirstOrDefaultAsync<VM>(Expression<Func<VM>> func)
            where VM:class
        {
            SelectMHandle(func);
            DC.DPH.SetParameter();
            DC.Method = UiMethodEnum.JoinFirstOrDefaultAsync;
            DC.SqlProvider.GetSQL();
            return await DC.DS.ExecuteReaderSingleRowAsync<VM>();
        }
    }
}
