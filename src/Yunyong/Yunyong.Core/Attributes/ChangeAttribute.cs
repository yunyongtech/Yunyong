using System;

namespace Yunyong.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ChangeAttribute : Attribute
    {
        public string ColumnName { get; set; } = string.Empty;
        public ChangeOption Option { get; set; } = ChangeOption.Add;
    }
}