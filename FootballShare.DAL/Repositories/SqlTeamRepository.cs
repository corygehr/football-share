using Dapper;
using FootballShare.Entities.Leagues;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="ITeamRepository"/>
    /// </summary>
    public class SqlTeamRepository : ITeamRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlTeamRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public SqlTeamRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public Task<Team> CreateAsync(Team entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Team entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[Teams]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Team>(query);
            }
        }

        public async Task<Team> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Teams]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Team>(
                    query,
                    new
                    {
                        id = entityId
                    }
                );
            }
        }

        public async Task<Team> GetAsync(Team entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString());
        }

        public Task<Team> UpdateAsync(Team entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
