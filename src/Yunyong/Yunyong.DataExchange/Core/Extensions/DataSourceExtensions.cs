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
        /// <summary>
        /// Attempts setup a <see cref="DbCommand"/> on a <see cref="DbConnection"/>, with a better error message for unsupported usages.
        /// </summary>
        internal static DbCommand TrySetupAsyncCommand(this CommandDefinition command, IDbConnection cnn, Action<IDbCommand, DbParameters> paramReader)
        {
            var cmd = cnn.CreateCommand();
            cmd.CommandText = command.CommandText;  // CommandText;
            cmd.CommandTimeout = XConfig.CommandTimeout;
            cmd.CommandType = CommandType.Text;
            paramReader?.Invoke(cmd,command.Parameters);  // (cmd, Parameters);
            return cmd as DbCommand;

            //if (command.SetupCommand(cnn, paramReader) is DbCommand dbCommand)
            //{
            //    return dbCommand;
            //}
            //else
            //{
            //    throw new InvalidOperationException("Async operations require use of a DbConnection or an IDbConnection where .CreateCommand() returns a DbCommand");
            //}
        }

        /// <summary>
        /// Attempts to open a connection asynchronously, with a better error message for unsupported usages.
        /// </summary>
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
