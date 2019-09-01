using Dapper;
using FootballShare.Entities.League;

using System;
using System.Collections.Generic;
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

        public async Task<Season> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Seasons]
                              WHERE [Id] = @id";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Season>(query, new
                {
                    id = id
                });
            }
        }

        public async Task<IEnumerable<SeasonWeek>> GetAllWeeksAsync(Season season, CancellationToken cancellationToken)
        {
            if(season == null)
            {
                throw new ArgumentNullException(nameof(season));
            }

            string query = $@"SELECT *
                              FROM [dbo].[SeasonWeeks]
                              WHERE [SeasonId] = @{nameof(Season.Id)} 
                              ORDER BY [Sequence]";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek>(query, season);
            }
        }

        public async Task<Season> GetCurrentForLeagueAsync(SportsLeague league, CancellationToken cancellationToken)
        {
            if(league == null)
            {
                throw new ArgumentNullException(nameof(league));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Seasons]
                              WHERE [LeagueId] = @{nameof(SportsLeague.Id)}
                              ORDER BY [StartDate] DESC";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Season>(query, league);
            }
        }
    }
}
