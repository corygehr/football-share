using Dapper;
using FootballShare.Entities.Leagues;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IWeekEventRepository"/>
    /// </summary>
    public class SqlWeekEventRepository : IWeekEventRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlWeekEventRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlWeekEventRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<WeekEvent> CreateAsync(WeekEvent entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[WeekEvents] (
                                [AwayScore],
                                [AwayTeamId],
                                [HomeScore],
                                [HomeTeamId],
                                [Overtime],
                                [Postponed],
                                [SeasonWeekId],
                                [Time],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(WeekEvent.AwayScore)},
                                @{nameof(WeekEvent.AwayTeamId)},
                                @{nameof(WeekEvent.HomeScore)},
                                @{nameof(WeekEvent.HomeTeamId)},
                                @{nameof(WeekEvent.Overtime)},
                                @{nameof(WeekEvent.Postponed)},
                                @{nameof(WeekEvent.SeasonWeekId)},
                                @{nameof(WeekEvent.Time)},
                                CURRENT_TIMESTAMP
                              );
                              SELECT 
                                TOP 1 [we].*,
                                [t_a].*,
                                [t_h].*,
                                [sw].*,
                                [sw_s].*
                              FROM [dbo].[WeekEvents] [we]
                              INNER JOIN [dbo].[Teams] [t_a]
                                ON [we].[AwayTeamId] = [t_a].[Id]
                              INNER JOIN [dbo].[Teams] [t_h]
                                ON [we].[HomeTeamId] = [t_h].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [sw]
                                ON [we].[SeasonWeekId] = [sw].[Id]
                              INNER JOIN [dbo].[Seasons] [sw_s]
                                ON [sw].[SeasonId] = [sw_s].[Id]
                              WHERE [we].[Id] = (CAST(SCOPE_IDENTITY() AS INT));";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<WeekEvent> result = await connection.QueryAsync<WeekEvent, Team, Team, SeasonWeek, Season, WeekEvent>(
                    query,
                    (weekEvent, awayTeam, homeTeam, seasonWeek, season) =>
                    {
                        weekEvent.AwayTeam = awayTeam;
                        weekEvent.HomeTeam = homeTeam;
                        weekEvent.Week = seasonWeek;
                        weekEvent.Week.Season = season;

                        return weekEvent;
                    },
                    entity
                );

                return result.FirstOrDefault();
            }
        }

        public async Task DeleteAsync(WeekEvent entity, CancellationToken cancellationToken = default)
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

            string query = $@"DELETE FROM [dbo].[WeekEvents]
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

        public async Task<WeekEvent> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string query = $@"SELECT
                                [we].*,
                                [t_a].*,
                                [t_h].*,
                                [sw].*,
                                [sw_s].*
                              FROM [dbo].[WeekEvents] [we]
                              INNER JOIN [dbo].[Teams] [t_a]
                                ON [we].[AwayTeamId] = [t_a].[Id]
                              INNER JOIN [dbo].[Teams] [t_h]
                                ON [we].[HomeTeamId] = [t_h].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [sw]
                                ON [we].[SeasonWeekId] = [sw].[Id]
                              INNER JOIN [dbo].[Seasons] [sw_s]
                                ON [sw].[SeasonId] = [sw_s].[Id]
                              WHERE [we].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<WeekEvent> result = await connection.QueryAsync<WeekEvent, Team, Team, SeasonWeek, Season, WeekEvent>(
                    query,
                    (weekEvent, awayTeam, homeTeam, seasonWeek, season) =>
                    {
                        weekEvent.AwayTeam = awayTeam;
                        weekEvent.HomeTeam = homeTeam;
                        weekEvent.Week = seasonWeek;
                        weekEvent.Week.Season = season;

                        return weekEvent;
                    },
                    new
                    {
                        id = id
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<WeekEvent>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [we].*,
                                [t_a].*,
                                [t_h].*,
                                [sw].*,
                                [sw_s].*
                              FROM [dbo].[WeekEvents] [we]
                              INNER JOIN [dbo].[Teams] [t_a]
                                ON [we].[AwayTeamId] = [t_a].[Id]
                              INNER JOIN [dbo].[Teams] [t_h]
                                ON [we].[HomeTeamId] = [t_h].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [sw]
                                ON [we].[SeasonWeekId] = [sw].[Id]
                              INNER JOIN [dbo].[Seasons] [sw_s]
                                ON [sw].[SeasonId] = [sw_s].[Id]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<WeekEvent, Team, Team, SeasonWeek, Season, WeekEvent>(
                    query,
                    (weekEvent, awayTeam, homeTeam, seasonWeek, season) =>
                    {
                        weekEvent.AwayTeam = awayTeam;
                        weekEvent.HomeTeam = homeTeam;
                        weekEvent.Week = seasonWeek;
                        weekEvent.Week.Season = season;

                        return weekEvent;
                    }
                );
            }
        }

        public async Task<WeekEvent> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT 
                                TOP 1 [we].*,
                                [t_a].*,
                                [t_h].*,
                                [sw].*,
                                [sw_s].*
                              FROM [dbo].[WeekEvents] [we]
                              INNER JOIN [dbo].[Teams] [t_a]
                                ON [we].[AwayTeamId] = [t_a].[Id]
                              INNER JOIN [dbo].[Teams] [t_h]
                                ON [we].[HomeTeamId] = [t_h].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [sw]
                                ON [we].[SeasonWeekId] = [sw].[Id]
                              INNER JOIN [dbo].[Seasons] [sw_s]
                                ON [sw].[SeasonId] = [sw_s].[Id]
                              WHERE [we].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<WeekEvent> result = await connection.QueryAsync<WeekEvent, Team, Team, SeasonWeek, Season, WeekEvent>(
                    query,
                    (weekEvent, awayTeam, homeTeam, seasonWeek, season) =>
                    {
                        weekEvent.AwayTeam = awayTeam;
                        weekEvent.HomeTeam = homeTeam;
                        weekEvent.Week = seasonWeek;
                        weekEvent.Week.Season = season;

                        return weekEvent;
                    },
                    new
                    {
                        id = entityId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<WeekEvent> GetAsync(WeekEvent entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<WeekEvent> GetWeekEventByWeekAndTeamsAsync(string weekId, string awayTeamId, string homeTeamId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(weekId))
            {
                throw new ArgumentNullException(nameof(weekId));
            }

            if(String.IsNullOrEmpty(awayTeamId))
            {
                throw new ArgumentNullException(nameof(awayTeamId));
            }

            if(String.IsNullOrEmpty(homeTeamId))
            {
                throw new ArgumentNullException(nameof(homeTeamId));
            }

            string query = $@"SELECT
                                TOP 1 [we].*,
                                [t_a].*,
                                [t_h].*,
                                [sw].*,
                                [sw_s].*
                              FROM [dbo].[WeekEvents] [we]
                              INNER JOIN [dbo].[Teams] [t_a]
                                ON [we].[AwayTeamId] = [t_a].[Id]
                              INNER JOIN [dbo].[Teams] [t_h]
                                ON [we].[HomeTeamId] = [t_h].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [sw]
                                ON [we].[SeasonWeekId] = [sw].[Id]
                              INNER JOIN [dbo].[Seasons] [sw_s]
                                ON [sw].[SeasonId] = [sw_s].[Id]
                              WHERE [we].[SeasonWeekId] = @weekId
                              AND [we].[AwayTeamId] = @awayTeamId 
                              AND [we].[HomeTeamId] = @homeTeamId";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<WeekEvent> result = await connection.QueryAsync<WeekEvent, Team, Team, SeasonWeek, Season, WeekEvent>(
                    query,
                    (weekEvent, awayTeam, homeTeam, seasonWeek, season) =>
                    {
                        weekEvent.AwayTeam = awayTeam;
                        weekEvent.HomeTeam = homeTeam;
                        weekEvent.Week = seasonWeek;
                        weekEvent.Week.Season = season;

                        return weekEvent;
                    },
                    new
                    {
                        weekId = weekId,
                        awayTeamId = awayTeamId,
                        homeTeamId = homeTeamId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<WeekEvent>> GetWeekEventsAsync(string weekId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [we].*,
                                [t_a].*,
                                [t_h].*,
                                [sw].*,
                                [sw_s].*
                              FROM [dbo].[WeekEvents] [we]
                              INNER JOIN [dbo].[Teams] [t_a]
                                ON [we].[AwayTeamId] = [t_a].[Id]
                              INNER JOIN [dbo].[Teams] [t_h]
                                ON [we].[HomeTeamId] = [t_h].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [sw]
                                ON [we].[SeasonWeekId] = [sw].[Id]
                              INNER JOIN [dbo].[Seasons] [sw_s]
                                ON [sw].[SeasonId] = [sw_s].[Id]
                              WHERE [we].[SeasonWeekId] = @id
                              ORDER BY [we].[Time]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<WeekEvent, Team, Team, SeasonWeek, Season, WeekEvent>(
                    query,
                    (weekEvent, awayTeam, homeTeam, seasonWeek, season) =>
                    {
                        weekEvent.AwayTeam = awayTeam;
                        weekEvent.HomeTeam = homeTeam;
                        weekEvent.Week = seasonWeek;
                        weekEvent.Week.Season = season;

                        return weekEvent;
                    },
                    new
                    {
                        id = weekId
                    }
                );
            }
        }

        public async Task<WeekEvent> UpdateAsync(WeekEvent entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[WeekEvents]
                              SET [AwayScore] = @{nameof(WeekEvent.AwayScore)},
                                  [AwayTeamId] = @{nameof(WeekEvent.AwayTeamId)},
                                  [HomeScore] = @{nameof(WeekEvent.HomeScore)},
                                  [HomeTeamId] = @{nameof(WeekEvent.HomeTeamId)},
                                  [Overtime] = @{nameof(WeekEvent.Overtime)},
                                  [Postponed] = @{nameof(WeekEvent.Postponed)},
                                  [SeasonWeekId] = @{nameof(WeekEvent.SeasonWeekId)},
                                  [Time] = @{nameof(WeekEvent.Time)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP 
                              WHERE [Id] = @{nameof(WeekEvent.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[WeekEvents]
                              WHERE [Id] = @{nameof(WeekEvent.Id)};";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<WeekEvent>(query, entity);
            }
        }
    }
}