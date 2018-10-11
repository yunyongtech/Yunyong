﻿using System.ComponentModel;

namespace Yunyong.DataExchange.Core.Enums
{
    internal enum OptionEnum
    {
        /// <summary>
        /// none
        /// </summary>
        [Description("<<<<<")]
        None,

        /// <summary>
        /// ""
        /// </summary>
        [Description("")]
        Insert,

        /// <summary>
        /// ""
        /// </summary>
        [Description("")]
        InsertTVP,

        /// <summary>
        /// =
        /// </summary>
        [Description("=")]
        Set,

        /// <summary>
        /// +
        /// </summary>
        [Description("+")]
        ChangeAdd,

        /// <summary>
        /// -
        /// </summary>
        [Description("-")]
        ChangeMinus,

        /// <summary>
        /// ""
        /// </summary>
        [Description("")]
        Column,
        ColumnAs,

        /// <summary>
        /// ""
        /// </summary>
        [Description("")]
        Compare,

        /// <summary>
        /// " like "
        /// </summary>
        [Description(" like ")]
        Like,

        /// <summary>
        /// " in "
        /// </summary>
        [Description(" in ")]
        In,
        InHelper,

        /// <summary>
        /// " count"
        /// </summary>
        [Description(" count")]
        Count,

        /// <summary>
        /// " char_length"
        /// </summary>
        [Description(" char_length")]
        CharLength,

        /// <summary>
        /// ""
        /// </summary>
        [Description("")]
        OneEqualOne,

        /// <summary>
        /// " is null "
        /// </summary>
        [Description(" is null ")]
        IsNull,

        /// <summary>
        /// " is not null "
        /// </summary>
        [Description(" is not null ")]
        IsNotNull,

        /// <summary>
        /// " asc "
        /// </summary>
        [Description(" asc ")]
        Asc,

        /// <summary>
        /// " desc "
        /// </summary>
        [Description(" desc ")]
        Desc
    }
}
