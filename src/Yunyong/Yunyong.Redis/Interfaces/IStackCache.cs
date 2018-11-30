using System;
using System.Collections.Generic;
using System.Text;
using Yunyong.Redis.Base;

namespace Yunyong.Redis.Interfaces
{
    public interface IStackCache
        :IListCache
    {
        bool StackPush<T>(string key, T value)
            where T : CacheValueBase;

        T StackPop<T>(string key)
            where T : CacheValueBase;
    }
}
