﻿using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Yunyong.DataExchange.AdoNet;

namespace Yunyong.DataExchange.Core.Extensions
{
    internal static class DataSourceExtensions
    {
        internal static DbCommand TrySetupAsyncCommand(this CommandInfo command, IDbConnection cnn, Action<IDbCommand, DbParamInfo> paramReader)
        {
            var cmd = cnn.CreateCommand();
            cmd.CommandText = command.CommandText;  // CommandText;
            cmd.CommandTimeout = XConfig.CommandTimeout;
            cmd.CommandType = CommandType.Text;
            paramReader?.Invoke(cmd,command.Parameters);  // (cmd, Parameters);
            return cmd as DbCommand;
        }

        internal static Task TryOpenAsync(this IDbConnection cnn, CancellationToken cancel)
        {
            if (cnn is DbConnection dbConn)
            {
                return dbConn.OpenAsync(cancel);
            }
            else
            {
                throw new InvalidOperationException("Async operations require use of a DbConnection or an already-open IDbConnection");
            }
        }
    }
}
