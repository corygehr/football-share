using Dapper;
using FootballShare.Entities.Pools;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="ILedgerEntryRepository"/>
    /// </summary>
    public class SqlLedgerEntryRepository : ILedgerEntryRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlLedgerEntryRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlLedgerEntryRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<LedgerEntry> CreateAsync(LedgerEntry entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[Ledger] (
                                [PoolId],
                                [SiteUserId],
                                [WagerId],
                                [StartingBalance],
                                [TransactionAmount],
                                [Description],
                                [NewBalance],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(LedgerEntry.PoolId)},
                                @{nameof(LedgerEntry.SiteUserId)},
                                @{nameof(LedgerEntry.WagerId)},
                                @{nameof(LedgerEntry.StartingBalance)},
                                @{nameof(LedgerEntry.TransactionAmount)},
                                @{nameof(LedgerEntry.Description)},
                                @{nameof(LedgerEntry.NewBalance)},
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[Ledger] 
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT))";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<LedgerEntry>(query, entity);
            }
        }

        public async Task DeleteAsync(LedgerEntry entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LedgerEntry>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[Ledger]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<LedgerEntry>(query);
            }
        }

        public async Task<LedgerEntry> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Ledger]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<LedgerEntry>(
                    query,
                    new
                    {
                        id = entityId
                    });
            }
        }

        public async Task<LedgerEntry> GetAsync(LedgerEntry entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<LedgerEntry>> GetEntriesForPoolAsync(int poolId, CancellationToken cancellationToken)
        {
            string query = $@"SELECT *
                              FROM [dbo].[Ledger]
                              WHERE [PoolId] = @poolId 
                              ORDER BY [WhenCreated]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<LedgerEntry>(
                    query,
                    new
                    {
                        poolId = poolId
                    }
                );
            }
        }

        public Task<LedgerEntry> UpdateAsync(LedgerEntry entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
