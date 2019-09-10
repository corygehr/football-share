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
    /// SQL Server implementation of <see cref="ISeasonWeekRepository"/>
    /// </summary>
    public class SqlSeasonWeekRepository : ISeasonWeekRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSeasonWeekRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlSeasonWeekRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public Task<SeasonWeek> CreateAsync(SeasonWeek entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(SeasonWeek entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SeasonWeek>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [sw].*,
                                [s].*,
                                [s_l].*,
                                [s_l_s].*
                              FROM [dbo].[SeasonWeeks] [sw]
                              INNER JOIN [dbo].[Seasons] [s]
                                ON [sw].[SeasonId] = [s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [s_l]
                                ON [s].[SportsLeagueId] = [s_l].[Id]
                              INNER JOIN [dbo].[Sports] [s_l_s]
                                ON [s_l].[SportId] = [s_l_s].[Id]";
            
            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek, Season, SportsLeague, Sport, SeasonWeek>(
                    query,
                    (seasonWeek, season, league, sport) =>
                    {
                        seasonWeek.Season = season;
                        seasonWeek.Season.League = league;
                        seasonWeek.Season.League.Sport = sport;
                        return seasonWeek;
                    }
                );
            }
        }

        public async Task<IEnumerable<SeasonWeek>> GetAllForSeasonAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(nameof(seasonId));
            }

            string query = $@"SELECT
                                [sw].*,
                                [s].*,
                                [s_l].*,
                                [s_l_s].*
                              FROM [dbo].[SeasonWeeks] [sw]
                              INNER JOIN [dbo].[Seasons] [s]
                                ON [sw].[SeasonId] = [s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [s_l]
                                ON [s].[SportsLeagueId] = [s_l].[Id]
                              INNER JOIN [dbo].[Sports] [s_l_s]
                                ON [s_l].[SportId] = [s_l_s].[Id]
                              WHERE [sw].[SeasonId] = @seasonId 
                              ORDER BY [sw].[Sequence]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek, Season, SportsLeague, Sport, SeasonWeek>(
                    query,
                    (seasonWeek, season, league, sport) =>
                    {
                        seasonWeek.Season = season;
                        seasonWeek.Season.League = league;
                        seasonWeek.Season.League.Sport = sport;
                        return seasonWeek;
                    },
                    new
                    {
                        seasonId = seasonId
                    }
                );
            }
        }

        public async Task<SeasonWeek> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [sw].*,
                                [s].*,
                                [s_l].*,
                                [s_l_s].*
                              FROM [dbo].[SeasonWeeks] [sw]
                              INNER JOIN [dbo].[Seasons] [s]
                                ON [sw].[SeasonId] = [s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [s_l]
                                ON [s].[SportsLeagueId] = [s_l].[Id]
                              INNER JOIN [dbo].[Sports] [s_l_s]
                                ON [s_l].[SportId] = [s_l_s].[Id]
                              WHERE [sw].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<SeasonWeek> result = await connection.QueryAsync<SeasonWeek, Season, SportsLeague, Sport, SeasonWeek>(
                    query,
                    (seasonWeek, season, league, sport) =>
                    {
                        seasonWeek.Season = season;
                        seasonWeek.Season.League = league;
                        seasonWeek.Season.League.Sport = sport;
                        return seasonWeek;
                    },
                    new
                    {
                        id = entityId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<SeasonWeek> GetAsync(SeasonWeek entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<SeasonWeek> GetCurrentSeasonWeekAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(nameof(seasonId));
            }

            string query = $@"SELECT
                                TOP 1 [sw].*,
                                [s].*,
                                [s_l].*,
                                [s_l_s].*
                              FROM [dbo].[SeasonWeeks] [sw]
                              INNER JOIN [dbo].[Seasons] [s]
                                ON [sw].[SeasonId] = [s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [s_l]
                                ON [s].[SportsLeagueId] = [s_l].[Id]
                              INNER JOIN [dbo].[Sports] [s_l_s]
                                ON [s_l].[SportId] = [s_l_s].[Id]
                              WHERE [sw].[SeasonId] = @seasonId 
                              AND [sw].[EndDate] > GETDATE() 
                              ORDER BY [sw].[StartDate]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<SeasonWeek> result = await connection.QueryAsync<SeasonWeek, Season, SportsLeague, Sport, SeasonWeek>(
                    query,
                    (seasonWeek, season, league, sport) =>
                    {
                        seasonWeek.Season = season;
                        seasonWeek.Season.League = league;
                        seasonWeek.Season.League.Sport = sport;
                        return seasonWeek;
                    },
                    new
                    {
                        seasonId = seasonId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<SeasonWeek>> GetPreviousSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(nameof(seasonId));
            }

            string query = $@"SELECT
                                TOP 1 [sw].*,
                                [s].*,
                                [s_l].*,
                                [s_l_s].*
                              FROM [dbo].[SeasonWeeks] [sw]
                              INNER JOIN [dbo].[Seasons] [s]
                                ON [sw].[SeasonId] = [s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [s_l]
                                ON [s].[SportsLeagueId] = [s_l].[Id]
                              INNER JOIN [dbo].[Sports] [s_l_s]
                                ON [s_l].[SportId] = [s_l_s].[Id]
                              WHERE [sw].[SeasonId] = @seasonId
                              AND [sw].[EndDate] < GETDATE()
                              ORDER BY [sw].[EndDate]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek, Season, SportsLeague, Sport, SeasonWeek>(
                    query,
                    (seasonWeek, season, league, sport) =>
                    {
                        seasonWeek.Season = season;
                        seasonWeek.Season.League = league;
                        seasonWeek.Season.League.Sport = sport;
                        return seasonWeek;
                    },
                    new
                    {
                        seasonId = seasonId
                    }
                );
            }
        }

        public Task<SeasonWeek> UpdateAsync(SeasonWeek entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
