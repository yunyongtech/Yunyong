﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Yunyong.DataExchange.Core.Common;

namespace Yunyong.DataExchange
{
    public static class CommonExtension
    {

        /// <summary>
        /// 深度复制 [[只要你能点出来这个方法,就可以对该对象进行深度复制~~]]
        /// </summary>
        public static T DeepClone<T>(this T obj)
        {
            var result = default(T);
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.SurrogateSelector = new SurrogateSelector();
                formatter.SurrogateSelector.ChainSelector(new NonSerialiazableTypeSurrogateSelector());
                var ms = new MemoryStream();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                result = (T)formatter.Deserialize(ms);
            }
            catch (Exception ex)
            {
                throw new Exception("方法:T DeepClone<T>(this T obj)出错.", ex);
            }
            return result;
        }

        /*****************************************************************************************************************************************/

        /// <summary>
        /// Is null/empty/whitespace ?
        /// </summary>
        public static bool IsNullStr(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /************************************************************************************************************************************************/

    }
}
