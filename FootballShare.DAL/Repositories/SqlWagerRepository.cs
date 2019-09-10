﻿using Dapper;
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

        public async Task<IEnumerable<Wager>> FindBySeasonUserAndPoolAsync(int poolId, string seasonId, string userId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(nameof(seasonId));
            }

            if(String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(userId);
            }

            string query = $@"SELECT
                                [w].*,
                                [we].*,
                                [we_at].*,
                                [we_ht].*,
                                [p].*,
                                [t].*,
                                [su].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [w].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Teams] [we_at]
                                ON [we].[AwayTeamId] = [we_at].[Id]
                              INNER JOIN [dbo].[Teams] [we_ht]
                                ON [we].[HomeTeamId] = [we_ht].[Id]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [w].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Teams] [t]
                                ON [w].[SelectedTeamId] = [t].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [w].[SiteUserId] = [su].[Id]
                              WHERE [w].[SeasonId] = @seasonId
                              AND [w].[PoolId] = @poolId
                              AND [w].[SiteUserId] = @userId
                              ORDER BY [we].[Time]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager, WeekEvent, Team, Team, Pool, Team, SiteUser, Wager>(
                    query,
                    (wager, weekEvent, awayTeam, homeTeam, pool, team, siteUser) =>
                    {
                        wager.Event = weekEvent;
                        wager.Event.AwayTeam = awayTeam;
                        wager.Event.HomeTeam = homeTeam;
                        wager.Pool = pool;
                        wager.SelectedTeam = team;
                        wager.User = siteUser;

                        return wager;
                    },
                    new
                    {
                        seasonId = seasonId,
                        poolId = poolId,
                        userId = userId
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

            string query = $@"SELECT [w].*, [we].*, [p].*, [t].*, [su].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [w].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [w].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Teams] [t]
                                ON [w].[SelectedTeamId] = [t].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [w].[SiteUserId] = [su].[Id]
                              WHERE [w].[SiteUserId] = @userId
                              AND [we].[SeasonWeekId] = @weekId
                              ORDER BY [we].[Time]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager, WeekEvent, Pool, Team, SiteUser, Wager>(
                    query,
                    (wager, weekEvent, pool, team, siteUser) =>
                    {
                        wager.Event = weekEvent;
                        wager.Pool = pool;
                        wager.SelectedTeam = team;
                        wager.User = siteUser;

                        return wager;
                    },
                    new
                    {
                        userId = userId,
                        weekId = weekId
                    }
                );
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

            string query = $@"SELECT
                                [w].*,
                                [we].*,
                                [we_at].*,
                                [we_ht].*,
                                [p].*,
                                [t].*,
                                [su].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [w].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Teams] [we_at]
                                ON [we].[AwayTeamId] = [we_at].[Id]
                              INNER JOIN [dbo].[Teams] [we_ht]
                                ON [we].[HomeTeamId] = [we_ht].[Id]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [w].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Teams] [t]
                                ON [w].[SelectedTeamId] = [t].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [w].[SiteUserId] = [su].[Id]
                              WHERE [w].[PoolId] = @poolId 
                              AND [w].[SiteUserId] = @userId
                              AND [we].[SeasonWeekId] = @weekId
                              ORDER BY [we].[Time]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager, WeekEvent, Team, Team, Pool, Team, SiteUser, Wager>(
                    query,
                    (wager, weekEvent, awayTeam, homeTeam, pool, team, siteUser) =>
                    {
                        wager.Event = weekEvent;
                        wager.Event.AwayTeam = awayTeam;
                        wager.Event.HomeTeam = homeTeam;
                        wager.Pool = pool;
                        wager.SelectedTeam = team;
                        wager.User = siteUser;

                        return wager;
                    },
                    new
                    {
                        poolId = poolId,
                        userId = userId,
                        weekId = weekId
                    }
                );
            }
        }

        public async Task<IEnumerable<Wager>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [w].*,
                                [we].*,
                                [we_at].*,
                                [we_ht].*,
                                [p].*,
                                [t].*,
                                [su].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [w].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Teams] [we_at]
                                ON [we].[AwayTeamId] = [we_at].[Id]
                              INNER JOIN [dbo].[Teams] [we_ht]
                                ON [we].[HomeTeamId] = [we_ht].[Id]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [w].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Teams] [t]
                                ON [w].[SelectedTeamId] = [t].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [w].[SiteUserId] = [su].[Id]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Wager, WeekEvent, Team, Team, Pool, Team, SiteUser, Wager>(
                    query,
                    (wager, weekEvent, awayTeam, homeTeam, pool, team, siteUser) =>
                    {
                        wager.Event = weekEvent;
                        wager.Event.AwayTeam = awayTeam;
                        wager.Event.HomeTeam = homeTeam;
                        wager.Pool = pool;
                        wager.SelectedTeam = team;
                        wager.User = siteUser;

                        return wager;
                    }
                );
            }
        }

        public async Task<Wager> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [w].*,
                                [we].*,
                                [we_at].*,
                                [we_ht].*,
                                [p].*,
                                [t].*,
                                [su].*
                              FROM [dbo].[Wagers] [w]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [w].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Teams] [we_at]
                                ON [we].[AwayTeamId] = [we_at].[Id]
                              INNER JOIN [dbo].[Teams] [we_ht]
                                ON [we].[HomeTeamId] = [we_ht].[Id]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [w].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Teams] [t]
                                ON [w].[SelectedTeamId] = [t].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [w].[SiteUserId] = [su].[Id]
                              WHERE [w].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Wager> result = await connection.QueryAsync<Wager, WeekEvent, Team, Team, Pool, Team, SiteUser, Wager>(
                    query,
                    (wager, weekEvent, awayTeam, homeTeam, pool, team, siteUser) =>
                    {
                        wager.Event = weekEvent;
                        wager.Event.AwayTeam = awayTeam;
                        wager.Event.HomeTeam = homeTeam;
                        wager.Pool = pool;
                        wager.SelectedTeam = team;
                        wager.User = siteUser;

                        return wager;
                    }
                );

                return result.FirstOrDefault();
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
