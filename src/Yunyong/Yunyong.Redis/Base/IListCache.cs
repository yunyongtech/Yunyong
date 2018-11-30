using System;
using System.Collections.Generic;
using System.Text;

namespace Yunyong.Redis.Base
{
    public interface IListCache
        :ICache
    {
        bool Delete(params string[] keys);
        int Length(string key);
    }
}
