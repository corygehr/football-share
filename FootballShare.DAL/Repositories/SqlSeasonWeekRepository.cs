using Dapper;
using FootballShare.Entities.League;

using System;
using System.Collections.Generic;
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
            string query = $@"SELECT *
                              FROM [dbo].[SeasonWeeks]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek>(query);
            }
        }

        public async Task<IEnumerable<SeasonWeek>> GetAllForSeasonAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(nameof(seasonId));
            }

            string query = $@"SELECT *
                              FROM [dbo].[SeasonWeeks]
                              WHERE [SeasonId] = @seasonId 
                              ORDER BY [Sequence]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek>(
                    query,
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

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SeasonWeeks]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SeasonWeek>(
                    query,
                    new {
                        id = entityId
                   }
                );
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

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SeasonWeeks]
                              WHERE [SeasonId] = @seasonId 
                              AND [EndDate] >= GETDATE() 
                              ORDER BY [StartDate]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SeasonWeek>(
                    query,
                    new
                    {
                        seasonId = seasonId
                    }
                );
            }
        }

        public async Task<IEnumerable<SeasonWeek>> GetPreviousSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(nameof(seasonId));
            }

            string query = $@"SELECT *
                              FROM [dbo].[SeasonWeeks]
                              WHERE [SeasonId] = @seasonId
                              AND [EndDate] < GETDATE()
                              ORDER BY [EndDate]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SeasonWeek>(
                    query,
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
