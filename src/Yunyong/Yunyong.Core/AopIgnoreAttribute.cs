using System;

namespace Yunyong.Core
{
    /// <summary>
    ///     AopIgnore
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class AopIgnoreAttribute : Attribute
    {
    }
}