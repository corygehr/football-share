using Dapper;
using FootballShare.Entities.Betting;
using FootballShare.Entities.League;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IWagerRepository"/>
    /// </summary>
    public class SqlWagerRepository : IWagerRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlWagerRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Databasec connection factory</param>
        public SqlWagerRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<Wager> CreateAsync(Wager entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[Wagers] (
                                [Amount],
                                [AwaySpread],
                                [HomeSpread],
                                [SiteUserId],
                                [Target],
                                [WeekEventId],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(Wager.Amount)},
                                @{nameof(Wager.AwaySpread)},
                                @{nameof(Wager.HomeSpread)},
                                @{nameof(Wager.SiteUserId)},
                                @{nameof(Wager.Target)},
                                @{nameof(Wager.WeekEventId)},
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[Wagers]
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT))";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Wager>(query, entity);
            }
        }

        public async Task DeleteAsync(Wager entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"DELETE FROM [dbo].[Wagers]
                              WHERE [SiteUserId] = @{nameof(Wager.SiteUserId)}
                              AND [WeekEventId] = @{nameof(Wager.WeekEventId)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Wager> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Wagers]
                              WHERE [Id] = @id";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Wager>(query, new
                {
                    id = id
                });
            }
        }

        public async Task<IEnumerable<Wager>> FindByWeekAndUser(SeasonWeek weekEvent, Guid userId, CancellationToken cancellationToken = default)
        {
            if(weekEvent == null)
            {
                throw new ArgumentNullException(nameof(weekEvent));
            }

            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT *
                              FROM [dbo].[Wagers]
                              WHERE [SiteUserId] = @userId
                              AND [WeekEventId] = @eventId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager>(query, new
                {
                    eventId = weekEvent.Id,
                    userId = userId
                });
            }
        }

        public async Task<IEnumerable<Wager>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[Wagers]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager>(query);
            }
        }

        public async Task<Wager> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Wagers]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Wager>(
                    query,
                    new
                    {
                        id = entityId
                    }
                );
            }
        }

        public Task<Wager> GetAsync(Wager entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task InsertWagerAsync(Wager wager, CancellationToken cancellationToken = default)
        {
            if(wager == null)
            {
                throw new ArgumentNullException(nameof(wager));
            }

            string query = $@"INSERT INTO [dbo].[Wagers] (
                                [Amount],
                                [AwaySpread],
                                [HomeSpread],
                                [SiteUserId],
                                [Target],
                                [WeekEventId],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(Wager.Amount)},
                                @{nameof(Wager.AwaySpread)},
                                @{nameof(Wager.HomeSpread)},
                                @{nameof(Wager.SiteUserId)},
                                @{nameof(Wager.Target)},
                                @{nameof(Wager.WeekEventId)},
                                CURRENT_TIMESTAMP
                              )";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, wager);
            }
        }

        public Task<Wager> UpdateAsync(Wager entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
