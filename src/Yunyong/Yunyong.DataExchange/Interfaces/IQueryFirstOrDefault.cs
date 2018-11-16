﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yunyong.DataExchange.Interfaces
{
    internal interface IQueryFirstOrDefault<M>
        where M : class
    {
        Task<M> FirstOrDefaultAsync();
        Task<VM> FirstOrDefaultAsync<VM>()
            where VM : class;
        Task<VM> FirstOrDefaultAsync<VM>(Expression<Func<M, VM>> func)
            where VM : class;
    }

    internal interface IQueryFirstOrDefaultX
    {
        Task<M> FirstOrDefaultAsync<M>()
            where M : class;
        Task<VM> FirstOrDefaultAsync<VM>(Expression<Func<VM>> func)
            where VM : class;
    }
}
