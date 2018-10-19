using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.Impls
{
    internal class QueryListImpl<M>
        : Impler, IQueryList<M>
        where M : class
    {
        internal QueryListImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<List<M>> QueryListAsync()
        {
            //return await QueryListAsyncHandle<M, M>();
            //SelectMHandle<M>();
            //DC.IP.ConvertDic();
            return (await DC.DS.ExecuteReaderMultiRowAsync<M>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.QueryListAsync)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }

        public async Task<List<VM>> QueryListAsync<VM>()
        {
            //return await QueryListAsyncHandle<M, VM>();
            SelectMHandle<M, VM>();
            DC.IP.ConvertDic();
            return (await DC.DS.ExecuteReaderMultiRowAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.QueryListAsync)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }

        public async Task<List<VM>> QueryListAsync<VM>(Expression<Func<M, VM>> func)
        {
            SelectMHandle(func);
            DC.IP.ConvertDic();
            //return await QueryListAsyncHandle<M, VM>();
            //SelectMHandle<M, VM>();
            //DC.IP.ConvertDic();
            return (await DC.DS.ExecuteReaderMultiRowAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.QueryListAsync)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }
    }

    internal class QueryListXImpl
        : Impler, IQueryListX
    {
        internal QueryListXImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<List<M>> QueryListAsync<M>()
        {
            SelectMHandle<M>();
            DC.IP.ConvertDic();
            return (await DC.DS.ExecuteReaderMultiRowAsync<M>(
                DC.Conn,
                DC.SqlProvider.GetSQL<M>(UiMethodEnum.JoinQueryListAsync)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }

        public async Task<List<VM>> QueryListAsync<VM>(Expression<Func<VM>> func)
        {
            SelectMHandle(func);
            DC.IP.ConvertDic();
            return (await DC.DS.ExecuteReaderMultiRowAsync<VM>(
                DC.Conn,
                DC.SqlProvider.GetSQL<VM>(UiMethodEnum.JoinQueryListAsync)[0],
                DC.SqlProvider.GetParameters())).ToList();
        }
    }
}
