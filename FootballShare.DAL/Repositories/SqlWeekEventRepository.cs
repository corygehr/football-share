using Dapper;
using FootballShare.Entities.League;

using System;
using System.Collections.Generic;
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

        public async Task<WeekEvent> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[WeekEvents]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<WeekEvent>(query, new
                {
                    id = id
                });
            }
        }

        public async Task<IEnumerable<WeekEvent>> GetAllForWeekAsync(SeasonWeek week, CancellationToken cancellationToken = default)
        {
            if(week == null)
            {
                throw new ArgumentNullException(nameof(week));
            }

            string query = $@"SELECT *
                              FROM [dbo].[WeekEvents]
                              WHERE [SeasonWeekId] = @{nameof(SeasonWeek.Id)}
                              ORDER BY [Time]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<WeekEvent>(query, week);
            }
        }
    }
}