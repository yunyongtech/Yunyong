﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Yunyong.DataExchange.AdoNet.Interfaces
{
    /// <summary>
    /// Implements this interface to provide custom member mapping
    /// </summary>
    public interface IMemberMap
    {
        /// <summary>
        /// Source DataReader column name
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        ///  Target member type
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// Target property
        /// </summary>
        PropertyInfo Property { get; }

        /// <summary>
        /// Target field
        /// </summary>
        FieldInfo Field { get; }

        /// <summary>
        /// Target constructor parameter
        /// </summary>
        ParameterInfo Parameter { get; }
    }
}
