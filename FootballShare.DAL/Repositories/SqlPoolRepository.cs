using Dapper;
using FootballShare.Entities.Pools;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IPoolRepository"/>
    /// </summary>
    public class SqlPoolRepository : IPoolRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlPoolRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public SqlPoolRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<Pool> CreateAsync(Pool entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[Pools] (
                                [SeasonId],
                                [Name],
                                [IsPublic],
                                [StartingBalance],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(Pool.SeasonId)},
                                @{nameof(Pool.Name)},
                                @{nameof(Pool.IsPublic)},
                                @{nameof(Pool.StartingBalance)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[Pools]
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT));";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Pool>(query, entity);
            }
        }

        public async Task DeleteAsync(Pool entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[Pools]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(
                    query,
                    new
                    {
                        id = entityId
                    }
                );
            }
        }

        public async Task<IEnumerable<Pool>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[Pools]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Pool>(query);
            }
        }

        public async Task<IEnumerable<Pool>> GetAllPublicAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[Pools]
                              WHERE [IsAdmin] = 1";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Pool>(query);
            }
        }

        public async Task<Pool> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Pools]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Pool>(
                    query,
                    new
                    {
                        id = entityId
                    }
                );
            }
        }

        public async Task<Pool> GetAsync(Pool entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<Pool> UpdateAsync(Pool entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[Pools]
                              SET [Name] = @{nameof(Pool.Name)},
                                  [IsPublic] = @{nameof(Pool.IsPublic)},
                                  [StartingBalance] = @{nameof(Pool.StartingBalance)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(Pool.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[Pools]
                              WHERE [Id] = @{nameof(Pool.Id)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Pool>(query, entity);
            }
        }
    }
}
