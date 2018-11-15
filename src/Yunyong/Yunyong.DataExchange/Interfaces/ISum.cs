﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Yunyong.DataExchange.Interfaces
{
    internal interface ISum<M>
        where M : class
    {
        Task<F> SumAsync<F>(Expression<Func<M, F>> propertyFunc)
            where F : struct;
    }
}
