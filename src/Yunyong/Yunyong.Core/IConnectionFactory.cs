using System.Data.Common;

namespace Yunyong.Core
{
    /// <summary>
    ///     IConnectionFactory
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        ///     获取连接
        /// </summary>
        DbConnection GetConnection(string connectionString, bool defaultOpen = true);
    }
}