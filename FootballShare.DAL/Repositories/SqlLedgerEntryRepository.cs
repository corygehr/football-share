using Dapper;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string query = $@"SELECT
                                [l].*,
                                [p].*,
                                [su].*,
                                [w].*,
                                [w_we].*
                              FROM [dbo].[Ledger] [l]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [l].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [l].[SiteUserId] = [su].[Id]
                              INNER JOIN [dbo].[Wagers] [w]
                                ON [l].[WagerId] = [w].[Id]
                              INNER JOIN [dbo].[WeekEvents] [w_we]
                                ON [w].[WeekEventId] = [w_we].[Id]
                              INNER JOIN [dbo].[Teams] [w_t]
                                ON [w].[SelectedTeamId] = [w_t].[Id]";
            
            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<LedgerEntry, Pool, SiteUser, Wager, WeekEvent, Team, LedgerEntry>(
                    query,
                    (entry, pool, user, wager, weekEvent, selectedTeam) =>
                    {
                        entry.Pool = pool;
                        entry.User = user;
                        entry.Wager = wager;
                        entry.Wager.Event = weekEvent;
                        entry.Wager.SelectedTeam = selectedTeam;
                        return entry;
                    }
                );
            }
        }

        public async Task<LedgerEntry> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [l].*,
                                [p].*,
                                [su].*,
                                [w].*,
                                [w_we].*
                              FROM [dbo].[Ledger] [l]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [l].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [l].[SiteUserId] = [su].[Id]
                              INNER JOIN [dbo].[Wagers] [w]
                                ON [l].[WagerId] = [w].[Id]
                              INNER JOIN [dbo].[WeekEvents] [w_we]
                                ON [w].[WeekEventId] = [w_we].[Id]
                              INNER JOIN [dbo].[Teams] [w_t]
                                ON [w].[SelectedTeamId] = [w_t].[Id]
                              WHERE [l].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<LedgerEntry> result = await connection.QueryAsync<LedgerEntry, Pool, SiteUser, Wager, WeekEvent, Team, LedgerEntry>(
                    query,
                    (entry, pool, user, wager, weekEvent, selectedTeam) =>
                    {
                        entry.Pool = pool;
                        entry.User = user;
                        entry.Wager = wager;
                        entry.Wager.Event = weekEvent;
                        entry.Wager.SelectedTeam = selectedTeam;
                        return entry;
                    },
                    new
                    {
                        id = entityId
                    });

                return result.FirstOrDefault();
            }
        }

        public async Task<LedgerEntry> GetAsync(LedgerEntry entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<LedgerEntry>> GetEntriesForPoolAsync(int poolId, CancellationToken cancellationToken)
        {
            string query = $@"SELECT
                                [l].*,
                                [p].*,
                                [su].*,
                                [w].*,
                                [w_we].*,
                                [w_t].*
                              FROM [dbo].[Ledger] [l]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [l].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [l].[SiteUserId] = [su].[Id]
                              LEFT OUTER JOIN [dbo].[Wagers] [w]
                                ON [l].[WagerId] = [w].[Id]
                              LEFT OUTER JOIN [dbo].[WeekEvents] [w_we]
                                ON [w].[WeekEventId] = [w_we].[Id]
                              LEFT OUTER JOIN [dbo].[Teams] [w_t]
                                ON [w].[SelectedTeamId] = [w_t].[Id]
                              WHERE [l].[PoolId] = @poolId 
                              ORDER BY [l].[WhenCreated] DESC";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<LedgerEntry, Pool, SiteUser, Wager, WeekEvent, Team, LedgerEntry>(
                    query,
                    (entry, pool, user, wager, weekEvent, selectedTeam) =>
                    {
                        entry.Pool = pool;
                        entry.User = user;
                        entry.Wager = wager;

                        if(entry.Wager != null)
                        {
                            entry.Wager.Event = weekEvent;
                            entry.Wager.SelectedTeam = selectedTeam;
                        }

                        return entry;
                    },
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
