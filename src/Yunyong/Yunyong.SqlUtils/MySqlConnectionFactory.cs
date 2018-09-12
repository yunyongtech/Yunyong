using System.Data.Common;
using MySql.Data.MySqlClient;
using Yunyong.Core;

namespace Yunyong.SqlUtils
{
    public class MySqlConnectionFactory : IConnectionFactory
    {
        private DbConnection Connection { get; set; }

        public DbConnection GetConnection(string connectionString, bool defaultOpen = true)
        {
            var conn = new MySqlConnection(connectionString);
            if (defaultOpen)
            {
                conn.Open();
            }

            return conn;
        }
    }
}