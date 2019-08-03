using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yunyong.Core.Attributes
{
    public class LookupAttribute : Attribute
    {
        public string TypeName { get; }
        public string DisplayField { get; }
        public string ValueField { get; }

        public LookupAttribute(string typeName, string displayField, string valueField)
        {
            TypeName = typeName;
            DisplayField = displayField;
            ValueField = valueField;
        }
    }
}
