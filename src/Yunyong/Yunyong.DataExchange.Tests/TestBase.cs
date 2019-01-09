using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Yunyong.DataExchange.Tests
{
    public abstract class TestBase : IDisposable
    {
        public void Dispose()
        {
            Conn.Close();
        }

        protected IDbConnection Conn
        {
            /*
             * CREATE DATABASE `EasyDAL_Exchange`;
             */
            get { return GetOpenConnection("YunyongDataExchangeTestDB_20190109"); }
        }

        private static IDbConnection GetOpenConnection(string name)
        {
            /*
             * 
            */
            var conn = new MySqlConnection($"Server=localhost; Database={name}; Uid=SkyUser; Pwd=Sky@4321;SslMode=none;");
            conn.Open();
            //Hints.Hint = true;  // 全局 Hint 配置, 生产环境不要开启 
            return conn;
        }
    }
}