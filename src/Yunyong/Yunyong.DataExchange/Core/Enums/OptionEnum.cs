﻿using System.ComponentModel;

namespace Yunyong.DataExchange.Core.Enums
{
    internal enum OptionEnum
    {
        /// <summary>
        /// none
        /// </summary>
        None,

        /// <summary>
        /// ""
        /// </summary>
        Insert,

        /// <summary>
        /// ""
        /// </summary>
        InsertTVP,

        /// <summary>
        /// =
        /// </summary>
        Set,

        /// <summary>
        /// +
        /// </summary>
        ChangeAdd,

        /// <summary>
        /// -
        /// </summary>
        ChangeMinus,

        /// <summary>
        /// ""
        /// </summary>
        Column,
        ColumnAs,

        /// <summary>
        /// " like "
        /// </summary>
        Like,

        /// <summary>
        /// " count"
        /// </summary>
        Count,

        /// <summary>
        /// " sum"
        /// </summary>
        Sum,
        
        /// <summary>
        /// " distinct "
        /// </summary>
        Distinct,

        /// <summary>
        /// ""
        /// </summary>
        Compare,

        /// <summary>
        /// ""
        /// </summary>
        Function,

        /// <summary>
        /// ""
        /// </summary>
        OneEqualOne,

        /// <summary>
        /// " is null "
        /// </summary>
        IsNull,

        /// <summary>
        /// " is not null "
        /// </summary>
        IsNotNull,

        /// <summary>
        /// " asc "
        /// </summary>
        Asc,

        /// <summary>
        /// " desc "
        /// </summary>
        Desc
    }
}
