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

        public Task<SportsLeague> CreateAsync(SportsLeague entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(SportsLeague entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(entity.Id, cancellationToken);
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SportsLeague>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = @"SELECT * FROM [dbo].[SportsLeagues]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SportsLeague>(query);
            }
        }

        public async Task<SportsLeague> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SportsLeagues]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SportsLeague>(query, new
                {
                    id = entityId
                });
            }
        }

        public async Task<SportsLeague> GetAsync(SportsLeague entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id, cancellationToken);
        }

        public Task<SportsLeague> UpdateAsync(SportsLeague entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
