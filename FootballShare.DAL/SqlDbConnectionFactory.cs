using System.Data;
using System.Data.SqlClient;

namespace FootballShare.DAL
{
    /// <summary>
    /// SQL Server implementation of <see cref="IDbConnectionFactory"/>
    /// </summary>
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// Database connection string
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Creates a new <see cref="SqlDbConnectionFactory"/> instance
        /// </summary>
        /// <param name="connectionString">Databse connection string</param>
        public SqlDbConnectionFactory(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection(this._connectionString);
            connection.Open();
            return connection;
        }
    }
}
