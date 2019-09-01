using Dapper;
using FootballShare.Entities.League;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="ISportsLeagueRepository"/>
    /// </summary>
    public class SqlSportsLeagueRepository : ISportsLeagueRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSportsLeagueRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Connection factory</param>
        public SqlSportsLeagueRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<SportsLeague> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SportsLeagues]
                              WHERE [Id] = @id";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SportsLeague>(query, new
                {
                    id = id
                });
            }
        }

        public async Task<IEnumerable<SportsLeague>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = @"SELECT * FROM [dbo].[SportsLeagues]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SportsLeague>(query);
            }
        }
    }
}
