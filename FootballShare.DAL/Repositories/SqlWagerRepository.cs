﻿using Dapper;
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
                                [PoolId],
                                [SiteUserId],
                                [WeekEventId],
                                [SelectedTeamId],
                                [Amount],
                                [SelectedTeamSpread],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(Wager.PoolId)},
                                @{nameof(Wager.SiteUserId)},
                                @{nameof(Wager.WeekEventId)},
                                @{nameof(Wager.SelectedTeamId)},
                                @{nameof(Wager.Amount)},
                                @{nameof(Wager.SelectedTeamSpread)},
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
            // Use overload
            await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[Wagers]
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

        public async Task<IEnumerable<Wager>> FindByWeekAndUserAsync(string weekId, Guid userId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(weekId))
            {
                throw new ArgumentNullException(nameof(weekId));
            }

            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT [w].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                [w].[WeekEventId] = [we].[Id]
                              WHERE [w].[SiteUserId] = @userId
                              AND [we].[SeasonWeekId] = @weekId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager>(query, new
                {
                    userId = userId,
                    weekId = weekId
                });
            }
        }

        public async Task<IEnumerable<Wager>> FindByWeekUserAndPoolAsync(int poolId, string weekId, Guid userId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(weekId))
            {
                throw new ArgumentNullException(nameof(weekId));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT [w].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [w].[WeekEventId] = [we].[Id]
                              WHERE [w].[PoolId] = @poolId 
                              AND [w].[SiteUserId] = @userId
                              AND [we].[SeasonWeekId] = @weekId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager>(query, new
                {
                    poolId = poolId,
                    userId = userId,
                    weekId = weekId
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

        public async Task<Wager> GetAsync(Wager entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public Task<Wager> UpdateAsync(Wager entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
