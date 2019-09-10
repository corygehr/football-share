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
    /// SQL Server implementation of <see cref="ISeasonRepository"/>
    /// </summary>
    public class SqlSeasonRepository : ISeasonRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSeasonRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlSeasonRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<Season> CreateAsync(Season entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[Seasons] (
                                [Id],
                                [EndDate],
                                [LeagueId],
                                [Name],
                                [StartDate],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(Season.Id)},
                                @{nameof(Season.EndDate)},
                                @{nameof(Season.League)},
                                @{nameof(Season.Name)},
                                @{nameof(Season.StartDate)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[Seasons]
                              WHERE [Id] = @{nameof(Season.Id)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Season>(query, entity);
            }
        }

        public async Task DeleteAsync(Season entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(entity.Id, cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[Seasons]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    id = entityId
                });
            }
        }

        public async Task<IEnumerable<Season>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [s].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Seasons] [s]
                              INNER JOIN [dbo].[SportsLeagues] [sl]
                                ON [s].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [dbo].[Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Season, SportsLeague, Sport, Season>(
                    query,
                    (season, league, sport) =>
                    {
                        season.League = league;
                        season.League.Sport = sport;
                        return season;
                    }
                );
            }
        }

        public async Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken)
        {
            string query = $@"SELECT
                                [s].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Seasons] [s]
                              INNER JOIN [dbo].[SportsLeagues] [sl]
                                ON [s].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [dbo].[Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]
                              WHERE [s].[EndDate] > GETDATE()";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Season, SportsLeague, Sport, Season>(
                    query,
                    (season, league, sport) =>
                    {
                        season.League = league;
                        season.League.Sport = sport;
                        return season;
                    }
                );
            }
        }

        public async Task<Season> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [s].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Seasons] [s]
                              INNER JOIN [dbo].[SportsLeagues] [sl]
                                ON [s].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [dbo].[Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]
                              WHERE [s].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Season> result = await connection.QueryAsync<Season, SportsLeague, Sport, Season>(
                    query,
                    (season, league, sport) =>
                    {
                        season.League = league;
                        season.League.Sport = sport;
                        return season;
                    },
                    new
                    {
                        id = entityId
                    });

                return result.FirstOrDefault();
            }
        }

        public async Task<Season> GetAsync(Season entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id, cancellationToken);
        }

        public async Task<Season> GetCurrentLeagueSeasonAsync(string leagueId, CancellationToken cancellationToken)
        {
            if(String.IsNullOrEmpty(leagueId))
            {
                throw new ArgumentNullException(nameof(leagueId));
            }

            string query = $@"SELECT
                                TOP 1 [s].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Seasons] [s]
                              INNER JOIN [dbo].[SportsLeagues] [sl]
                                ON [s].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [dbo].[Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]
                              WHERE [s].[LeagueId] = @id 
                              AND [s].[EndDate] > (GETDATE())
                              ORDER BY [s].[StartDate] DESC";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Season> result = await connection.QueryAsync<Season, SportsLeague, Sport, Season>(
                    query,
                    (season, league, sport) =>
                    {
                        season.League = league;
                        season.League.Sport = sport;
                        return season;
                    },
                    new
                    {
                        id = leagueId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<Season> UpdateAsync(Season entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[Seasons]
                              SET [EndDate] = @{nameof(Season.EndDate)},
                                  [SportsLeagueId] = @{nameof(Season.SportsLeagueId)},
                                  [Name] = @{nameof(Season.Name)},
                                  [StartDate] = @{nameof(Season.StartDate)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(Season.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[Seasons]
                              WHERE [Id] = @{nameof(Season.Id)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Season>(query, entity);
            }
        }
    }
}
