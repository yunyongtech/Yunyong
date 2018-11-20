﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yunyong.DataExchange.Interfaces
{
    internal interface IAll<M>
        where M : class
    {
        Task<List<M>> AllAsync();
        Task<List<VM>> AllAsync<VM>()
            where VM : class;
        Task<List<F>> AllAsync<F>(Expression<Func<M, F>> propertyFunc);
    }
}
