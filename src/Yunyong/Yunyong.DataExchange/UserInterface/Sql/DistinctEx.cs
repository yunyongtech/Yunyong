using Yunyong.DataExchange.UserFacade.Query;

namespace Yunyong.DataExchange
{
    public static class DistinctEx
    {
        public static DistinctQ<M> Distinct<M>(this Selecter<M> selecter)
            where M : class
        {
            selecter.DistinctHandle();
            return new DistinctQ<M>(selecter.DC);
        }

        public static DistinctQ<M> Distinct<M>(this WhereQ<M> where)
            where M : class
        {
            where.DistinctHandle();
            return new DistinctQ<M>(where.DC);
        }

        public static DistinctQ<M> Distinct<M>(this OrderByQ<M> orderBy)
            where M : class
        {
            orderBy.DistinctHandle();
            return new DistinctQ<M>(orderBy.DC);
        }

        public static DistinctQ<M> Distinct<M>(this ThenOrderByQ<M> orderBy)
            where M : class
        {
            orderBy.DistinctHandle();
            return new DistinctQ<M>(orderBy.DC);
        }
    }
}
