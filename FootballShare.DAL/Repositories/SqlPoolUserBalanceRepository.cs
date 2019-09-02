using Dapper;
using FootballShare.Entities.Betting;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IPoolUserBalanceRepository"/>
    /// </summary>
    public class SqlPoolUserBalanceRepository : IPoolUserBalanceRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlPoolUserBalanceRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlPoolUserBalanceRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<PoolUserBalance> CreateAsync(PoolUserBalance entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[PoolUserBalances] (
                                [SiteUserId],
                                [BettingGroupPoolId],
                                [Balance],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(PoolUserBalance.SiteUserId)},
                                @{nameof(PoolUserBalance.BettingGroupPoolId)},
                                @{nameof(PoolUserBalance.Balance)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[PoolUserBalances]
                              WHERE [SiteUserId] = @{nameof(PoolUserBalance.SiteUserId)}
                              AND [BettingGroupPoolId] = @{nameof(PoolUserBalance.BettingGroupPoolId)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolUserBalance>(query, entity);
            }
        }

        public async Task DeleteAsync(PoolUserBalance entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"DELETE FROM [dbo].[PoolUserBalances]
                              WHERE [SiteUserId] = @{nameof(PoolUserBalance.SiteUserId)}
                              AND [BettingGroupPoolId] = @{nameof(PoolUserBalance.BettingGroupPoolId)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PoolUserBalance>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PoolUserBalance> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<PoolUserBalance> GetAsync(PoolUserBalance entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[PoolUserBalances]
                              WHERE [SiteUserId] = @{nameof(PoolUserBalance.SiteUserId)}
                              AND [BettingGroupPoolId] = @{nameof(PoolUserBalance.BettingGroupPoolId)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolUserBalance>(query, entity);
            }
        }

        public async Task<PoolUserBalance> UpdateAsync(PoolUserBalance entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[PoolUserBalances]
                              SET [Balance] = @{nameof(PoolUserBalance.Balance)}
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [SiteUserId] = @{nameof(PoolUserBalance.SiteUserId)}
                              AND [BettingGroupPoolId] = @{nameof(PoolUserBalance.BettingGroupPoolId)};
                              SELECT TOP 1 *
                              FROM [dbo].[PoolUserBalances]
                              WHERE [SiteUserId] = @{nameof(PoolUserBalance.SiteUserId)}
                              AND [BettingGroupPoolId] = @{nameof(PoolUserBalance.BettingGroupPoolId)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolUserBalance>(query, entity);
            }
        }

        public async Task<bool> UserHasBalanceAsync(string userId, int poolId, double requiredBalance = 0, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT COUNT(*)
                              FROM [dbo].[PoolUserBalances]
                              WHERE [BettingGroupPoolId] = @poolId 
                              AND [SiteUserId] = @userId
                              AND [Balance] > @minBalance";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                int matchingRows = await connection.ExecuteScalarAsync<int>(
                    query,
                    new
                    {
                        poolId = poolId,
                        userId = userId,
                        minBalance = requiredBalance
                    }
                );

                return matchingRows > 0;
            }
        }
    }
}
