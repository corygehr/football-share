using System.Data;

namespace FootballShare.DAL
{
    /// <summary>
    /// Interface used for building <see cref="IDbConnection/> instances
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates and opens an <see cref="IDbConnection"/>
        /// </summary>
        /// <returns>Database connection object</returns>
        IDbConnection CreateConnection();
    }
}