using System;

namespace Yunyong.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionAttribute : Attribute
    {
        public string ColumnName { get; set; } = string.Empty;
        public ConditionType ConditionType { get; set; } = ConditionType.Equal;
    }
}