using Dapper;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="ISpreadRepository"/>
    /// </summary>
    public class SqlSpreadRepository : ISpreadRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSpreadRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlSpreadRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<Spread> CreateAsync(Spread entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Confirm a spread does not already exist for the provided week
            Spread existing = await this.GetByWeekEventAsync(entity.WeekEventId, cancellationToken);

            if (existing == null)
            {
                string query = $@"INSERT INTO [dbo].[Spreads] (
                                    [AwaySpread],
                                    [HomeSpread],
                                    [WeekEventId],
                                    [WhenCreated]
                                  VALUES (
                                    @{nameof(Spread.AwaySpread)},
                                    @{nameof(Spread.HomeSpread)},
                                    @{nameof(Spread.WeekEventId)},
                                    CURRENT_TIMESTAMP
                                  );
                                  SELECT TOP 1 *
                                  FROM [dbo].[Spreads]
                                  WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT);";

                using (var connection = this._connectionFactory.CreateConnection())
                {
                    return await connection.QuerySingleAsync<Spread>(query, entity);
                }
            }
            else
            {
                throw new ConstraintException("Cannot have multiple spreads for a given WeekEvent.");
            }
        }

        public async Task DeleteAsync(Spread entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[Spreads]
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

        public Task<IEnumerable<Spread>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Spread> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [s].*,
                                [we].*,
                                [we_at].*,
                                [we_ht].*,
                                [we_sw].*
                              FROM [dbo].[Spreads] [s]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [s].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Teams] [we_at]
                                ON [we].[AwayTeamId] = [we_at].[Id]
                              INNER JOIN [dbo].[Teams] [we_ht]
                                ON [we].[HomeTeamId] = [we_ht].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [we_sw]
                                ON [we].[SeasonWeekId] = [we_sw].[Id]
                              WHERE [s].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Spread> result = await connection.QueryAsync<Spread, WeekEvent, Team, Team, SeasonWeek, Spread>(
                    query,
                    (spread, weekEvent, awayTeam, homeTeam, seasonWeek) =>
                    {
                        spread.Event = weekEvent;
                        spread.Event.AwayTeam = awayTeam;
                        spread.Event.HomeTeam = homeTeam;
                        spread.Event.Week = seasonWeek;

                        return spread;
                    },
                    new
                    {
                        id = entityId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<Spread> GetAsync(Spread entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<Spread> GetByWeekEventAsync(int weekEventId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                TOP 1 [s].*,
                                [we].*,
                                [we_at].*,
                                [we_ht].*,
                                [we_sw].*
                              FROM [dbo].[Spreads] [s]
                              INNER JOIN [dbo].[WeekEvents] [we]
                                ON [s].[WeekEventId] = [we].[Id]
                              INNER JOIN [dbo].[Teams] [we_at]
                                ON [we].[AwayTeamId] = [we_at].[Id]
                              INNER JOIN [dbo].[Teams] [we_ht]
                                ON [we].[HomeTeamId] = [we_ht].[Id]
                              INNER JOIN [dbo].[SeasonWeeks] [we_sw]
                                ON [we].[SeasonWeekId] = [we_sw].[Id]
                              WHERE [s].[WeekEventId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Spread> result = await connection.QueryAsync<Spread, WeekEvent, Team, Team, SeasonWeek, Spread>(
                    query,
                    (spread, weekEvent, awayTeam, homeTeam, seasonWeek) =>
                    {
                        spread.Event = weekEvent;
                        spread.Event.AwayTeam = awayTeam;
                        spread.Event.HomeTeam = homeTeam;
                        spread.Event.Week = seasonWeek;

                        return spread;
                    },
                    new
                    {
                        id = weekEventId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<Spread> UpdateAsync(Spread entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[Spreads]
                              SET [AwaySpread] = @{nameof(Spread.AwaySpread)},
                                  [HomeSpread] = @{nameof(Spread.HomeSpread)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(Spread.Id)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                int result = await connection.ExecuteAsync(query, entity);

                if(result > 0)
                {
                    return await this.GetAsync(entity, cancellationToken);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task UpsertAsync(Spread spread, CancellationToken cancellationToken = default)
        {
            if(spread == null)
            {
                throw new ArgumentNullException(nameof(spread));
            }

            // Check if a spread exists for the Week Event
            if(await this.GetByWeekEventAsync(spread.WeekEventId) != null)
            {
                // Update
                await this.UpdateAsync(spread, cancellationToken);
            }
            else
            {
                // Create
                await this.CreateAsync(spread, cancellationToken);
            }
        }
    }
}
