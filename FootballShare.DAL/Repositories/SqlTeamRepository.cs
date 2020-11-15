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
    /// SQL Server implementation of <see cref="ITeamRepository"/>
    /// </summary>
    public class SqlTeamRepository : ITeamRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlTeamRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public SqlTeamRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public Task<Team> CreateAsync(Team entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Team entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [t].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Teams] [t]
                              INNER JOIN [SportsLeagues] [sl]
                                ON [t].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Team, SportsLeague, Sport, Team>(
                    query,
                    (team, league, sport) =>
                    {
                        team.League = league;
                        team.League.Sport = sport;
                        return team;
                    }
                );
            }
        }

        public async Task<Team> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [t].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Teams] [t]
                              INNER JOIN [SportsLeagues] [sl]
                                ON [t].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]
                              WHERE [t].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Team> result = await connection.QueryAsync<Team, SportsLeague, Sport, Team>(
                    query,
                    (team, league, sport) =>
                    {
                        team.League = league;
                        team.League.Sport = sport;
                        return team;
                    },
                    new
                    {
                        id = entityId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<Team> GetAsync(Team entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<Team> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            string query = $@"SELECT
                                TOP 1 [t].*,
                                [sl].*,
                                [sl_s].*
                              FROM [dbo].[Teams] [t]
                              INNER JOIN [SportsLeagues] [sl]
                                ON [t].[SportsLeagueId] = [sl].[Id]
                              INNER JOIN [Sports] [sl_s]
                                ON [sl].[SportId] = [sl_s].[Id]
                              WHERE [t].[Name] = @name";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<Team> result = await connection.QueryAsync<Team, SportsLeague, Sport, Team>(
                    query,
                    (team, league, sport) =>
                    {
                        team.League = league;
                        team.League.Sport = sport;
                        return team;
                    },
                    new
                    {
                        name = name
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public Task<Team> UpdateAsync(Team entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
