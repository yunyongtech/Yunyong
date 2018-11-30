using System;
using System.Collections.Generic;
using System.Text;

namespace Yunyong.Redis.Base
{
    public interface ICache
    {
        bool Expire(TimeSpan validTimeSpan, params string[] key);
        bool Rename(string oldKey, string newKey);
    }
}
