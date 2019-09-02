using Dapper;
using FootballShare.Entities.Pools;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IPoolMemberRepository"/>
    /// </summary>
    public class SqlPoolMemberRepository : IPoolMemberRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;
        
        /// <summary>
        /// Creates a new <see cref="SqlPoolMemberRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public SqlPoolMemberRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<PoolMember> CreateAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[PoolMembers] (
                                [PoolId],
                                [SiteUserId],
                                [IsAdmin],
                                [Balance],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(PoolMember.PoolId)},
                                @{nameof(PoolMember.SiteUserId)},
                                @{nameof(PoolMember.IsAdmin)},
                                @{nameof(PoolMember.Balance)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[PoolMembers]
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT));";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolMember>(query, entity);
            }
        }

        public async Task DeleteAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[PoolMembers]
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

        public async Task<IEnumerable<PoolMember>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[PoolMembers]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<PoolMember>(query);
            }
        }

        public async Task<PoolMember> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[PoolMembers]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolMember>(
                    query,
                    new
                    {
                        id = entityId
                    }
                );
            }
        }

        public async Task<PoolMember> GetAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<PoolMember>> GetPoolMembersAsync(int poolId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[PoolMembers]
                              WHERE [PoolId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<PoolMember>(
                    query,
                    new
                    {
                        id = poolId
                    }
                );
            }
        }

        public async Task<IEnumerable<PoolMember>> GetUserMembershipsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT *
                              FROM [dbo].[PoolMembers]
                              WHERE [SiteUserId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<PoolMember>(
                    query,
                    new
                    {
                        id = userId
                    }
                );
            }
        }

        public async Task<PoolMember> UpdateAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[PoolMembers]
                              SET [IsAdmin] = @{nameof(PoolMember.IsAdmin)},
                                  [Balance] = @{nameof(PoolMember.Balance)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(PoolMember.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[PoolMembers]
                              WHERE [Id] = @{nameof(PoolMember.Id)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolMember>(query, entity);
            }
        }
    }
}
