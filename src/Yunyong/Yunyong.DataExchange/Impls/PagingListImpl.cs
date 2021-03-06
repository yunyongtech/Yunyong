using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Yunyong.Core;
using Yunyong.DataExchange.Core.Bases;
using Yunyong.DataExchange.Core.Enums;
using Yunyong.DataExchange.Core.Extensions;
using Yunyong.DataExchange.Interfaces;

namespace Yunyong.DataExchange.Impls
{
    internal class PagingListImpl<M>
        : Impler, IPagingList<M>
            where M : class
    {
        internal PagingListImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<PagingList<M>> PagingListAsync(int pageIndex, int pageSize)
        {
            DC.PageIndex = pageIndex;
            DC.PageSize = pageSize;
            return await PagingListAsyncHandle<M>(UiMethodEnum.PagingListAsync, false);
        }

        public async Task<PagingList<VM>> PagingListAsync<VM>(int pageIndex, int pageSize)
            where VM : class
        {
            DC.PageIndex = pageIndex;
            DC.PageSize = pageSize;
            return await PagingListAsyncHandle<M, VM>(UiMethodEnum.PagingListAsync, false, null);
        }

        public async Task<PagingList<T>> PagingListAsync<T>(int pageIndex, int pageSize, Expression<Func<M, T>> columnMapFunc)
        {
            DC.PageIndex = pageIndex;
            DC.PageSize = pageSize;
            var single = typeof(T).IsSingleColumn();
            if (single)
            {
                SingleColumnHandle(columnMapFunc);
            }
            else
            {
                SelectMHandle(columnMapFunc);
            }
            return await PagingListAsyncHandle<M, T>(UiMethodEnum.PagingListAsync, single, columnMapFunc.Compile());
        }
    }

    internal class PagingListOImpl<M>
        : Impler, IPagingListO<M>
            where M : class
    {
        internal PagingListOImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<PagingList<M>> PagingListAsync(PagingQueryOption option)
        {
            DC.PageIndex = option.PageIndex;
            DC.PageSize = option.PageSize;
            OrderByOptionHandle(option, typeof(M).FullName);
            return await PagingListAsyncHandle<M>(UiMethodEnum.PagingListAsync, false);
        }

        public async Task<PagingList<VM>> PagingListAsync<VM>(PagingQueryOption option)
            where VM : class
        {
            DC.PageIndex = option.PageIndex;
            DC.PageSize = option.PageSize;
            SelectMHandle<M, VM>();
            OrderByOptionHandle(option, typeof(M).FullName);
            return await PagingListAsyncHandle<M, VM>(UiMethodEnum.PagingListAsync, false, null);
        }

        public async Task<PagingList<T>> PagingListAsync<T>(PagingQueryOption option, Expression<Func<M, T>> columnMapFunc)
        {
            DC.PageIndex = option.PageIndex;
            DC.PageSize = option.PageSize;
            var single = typeof(T).IsSingleColumn();
            if (single)
            {
                SingleColumnHandle(columnMapFunc);
            }
            else
            {
                SelectMHandle(columnMapFunc);
            }
            OrderByOptionHandle(option, typeof(M).FullName);
            return await PagingListAsyncHandle<M, T>(UiMethodEnum.PagingListAsync, single, columnMapFunc.Compile());
        }
    }

    internal class PagingListXImpl
        : Impler, IPagingListX
    {
        internal PagingListXImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<PagingList<M>> PagingListAsync<M>(int pageIndex, int pageSize)
            where M : class
        {
            DC.PageIndex = pageIndex;
            DC.PageSize = pageSize;
            SelectMHandle<M>();
            return await PagingListAsyncHandle<M>(UiMethodEnum.PagingListAsync, false);
        }

        public async Task<PagingList<T>> PagingListAsync<T>(int pageIndex, int pageSize, Expression<Func<T>> columnMapFunc)
        {
            DC.PageIndex = pageIndex;
            DC.PageSize = pageSize;
            var single = typeof(T).IsSingleColumn();
            if (single)
            {
                SingleColumnHandle(columnMapFunc);
            }
            else
            {
                SelectMHandle(columnMapFunc);
            }
            return await PagingListAsyncHandle<T>(UiMethodEnum.PagingListAsync, single);
        }
    }

    internal class PagingListXOImpl
        : Impler, IPagingListXO
    {
        internal PagingListXOImpl(Context dc)
            : base(dc)
        {
        }

        public async Task<PagingList<M>> PagingListAsync<M>(PagingQueryOption option)
            where M : class
        {
            DC.PageIndex = option.PageIndex;
            DC.PageSize = option.PageSize;
            SelectMHandle<M>();
            OrderByOptionHandle(option, typeof(M).FullName);
            return await PagingListAsyncHandle<M>(UiMethodEnum.PagingListAsync, false);
        }

        public async Task<PagingList<T>> PagingListAsync<T>(PagingQueryOption option, Expression<Func<T>> columnMapFunc)
        {
            DC.PageIndex = option.PageIndex;
            DC.PageSize = option.PageSize;
            var single = typeof(T).IsSingleColumn();
            if (single)
            {
                SingleColumnHandle(columnMapFunc);
            }
            else
            {
                SelectMHandle(columnMapFunc);
            }
            OrderByOptionHandle(option, string.Empty);
            return await PagingListAsyncHandle<T>(UiMethodEnum.PagingListAsync, single);
        }
    }
}
