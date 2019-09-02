using Dapper;
using FootballShare.Entities.Betting;

using System;
using System.Collections.Generic;
using System.Data;
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

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Spreads]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<Spread>(
                    query,
                    new
                    {
                        id = entityId
                    }
                );
            }
        }

        public async Task<Spread> GetAsync(Spread entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<Spread> GetByWeekEventAsync(string weekEventId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(weekEventId))
            {
                throw new ArgumentNullException(nameof(weekEventId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[Spreads]
                              WHERE [WeekEventId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Spread>(
                    query,
                    new
                    {
                        id = weekEventId
                    }
                );
            }
        }

        public Task<Spread> UpdateAsync(Spread entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
