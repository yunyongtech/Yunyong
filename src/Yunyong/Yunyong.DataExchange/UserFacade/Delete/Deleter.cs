using Yunyong.DataExchange.Common;
using Yunyong.DataExchange.Core;

namespace Yunyong.DataExchange.UserFacade.Delete
{
    public class Deleter<M> : Operator, IMethodObject
    {
        internal Deleter(Context dc)
            : base(dc)
        { }


    }
}
