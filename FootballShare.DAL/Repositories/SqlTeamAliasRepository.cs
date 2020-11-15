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
    /// SQL Server implementation of <see cref="ITeamAliasRepository"/>
    /// </summary>
    public class SqlTeamAliasRepository : ITeamAliasRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlTeamAliasRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public SqlTeamAliasRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        /// <inheritdoc/>
        public async Task<TeamAlias> CreateAsync(TeamAlias entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[TeamAliases] (
                                [TeamId],
                                [Alias],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(TeamAlias.TeamId)},
                                @{nameof(TeamAlias.Alias)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 [a].*
                              FROM [dbo].[TeamAliases] [a]
                              INNER JOIN [Teams] [a_t]
                                ON [a].[TeamId] = [a_t].[Id]
                              INNER JOIN [SportsLeagues] [a_t_sl]
                                ON [a_t].[SportsLeagueId] = [a_t_sl].[Id]
                              INNER JOIN [Sports] [a_t_sl_s]
                                ON [a_t_sl].[SportId] = [a_t_sl_s].[Id]
                              WHERE [a].[Id] = (CAST(SCOPE_IDENTITY() AS INT));";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<TeamAlias>(query, entity);
            }
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(TeamAlias entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[TeamAliases]
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

        /// <inheritdoc/>
        public async Task<IEnumerable<TeamAlias>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [a].[*],
                                [a_t].[*],
                                [a_t_sl].[*],
                                [a_t_sl_s].[*]
                              FROM [dbo].[TeamAliases] [a]
                              INNER JOIN [Teams] [a_t]
                                ON [a].[TeamId] = [a_t].[Id]
                              INNER JOIN [SportsLeagues] [a_t_sl]
                                ON [a_t].[SportsLeagueId] = [a_t_sl].[Id]
                              INNER JOIN [Sports] [a_t_sl_s]
                                ON [a_t_sl].[SportId] = [a_t_sl_s].[Id]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<TeamAlias, Team, SportsLeague, Sport, TeamAlias>(
                    query,
                    (alias, team, league, sport) =>
                    {
                        alias.Team = team;
                        alias.Team.League = league;
                        alias.Team.League.Sport = sport;
                        return alias;
                    }
                );
            }
        }

        /// <inheritdoc/>
        public async Task<TeamAlias> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [a].[*],
                                [a_t].[*],
                                [a_t_sl].[*],
                                [a_t_sl_s].[*]
                              FROM [dbo].[TeamAliases] [a]
                              INNER JOIN [Teams] [a_t]
                                ON [a].[TeamId] = [a_t].[Id]
                              INNER JOIN [SportsLeagues] [a_t_sl]
                                ON [a_t].[SportsLeagueId] = [a_t_sl].[Id]
                              INNER JOIN [Sports] [a_t_sl_s]
                                ON [a_t_sl].[SportId] = [a_t_sl_s].[Id]
                              WHERE [a].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<TeamAlias> result = await connection.QueryAsync<TeamAlias, Team, SportsLeague, Sport, TeamAlias>(
                    query,
                    (alias, team, league, sport) =>
                    {
                        alias.Team = team;
                        alias.Team.League = league;
                        alias.Team.League.Sport = sport;
                        return alias;
                    },
                    new
                    {
                        id = entityId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        /// <inheritdoc/>
        public async Task<TeamAlias> GetAsync(TeamAlias entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TeamAlias>> GetByAliasAsync(string teamAlias, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [a].[*],
                                [a_t].[*],
                                [a_t_sl].[*],
                                [a_t_sl_s].[*]
                              FROM [dbo].[TeamAliases] [a]
                              INNER JOIN [Teams] [a_t]
                                ON [a].[TeamId] = [a_t].[Id]
                              INNER JOIN [SportsLeagues] [a_t_sl]
                                ON [a_t].[SportsLeagueId] = [a_t_sl].[Id]
                              INNER JOIN [Sports] [a_t_sl_s]
                                ON [a_t_sl].[SportId] = [a_t_sl_s].[Id]
                              WHERE [a].[Alias] = @teamAlias";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<TeamAlias> result = await connection.QueryAsync<TeamAlias, Team, SportsLeague, Sport, TeamAlias>(
                    query,
                    (alias, team, league, sport) =>
                    {
                        alias.Team = team;
                        alias.Team.League = league;
                        alias.Team.League.Sport = sport;
                        return alias;
                    },
                    new
                    {
                        teamAlias = teamAlias
                    }
                );

                return result;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TeamAlias>> GetTeamAliasesAsync(string teamId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [a].[*],
                                [a_t].[*],
                                [a_t_sl].[*],
                                [a_t_sl_s].[*]
                              FROM [dbo].[TeamAliases] [a]
                              INNER JOIN [Teams] [a_t]
                                ON [a].[TeamId] = [a_t].[Id]
                              INNER JOIN [SportsLeagues] [a_t_sl]
                                ON [a_t].[SportsLeagueId] = [a_t_sl].[Id]
                              INNER JOIN [Sports] [a_t_sl_s]
                                ON [a_t_sl].[SportId] = [a_t_sl_s].[Id]
                              WHERE [a].[TeamId] = @teamId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<TeamAlias> result = await connection.QueryAsync<TeamAlias, Team, SportsLeague, Sport, TeamAlias>(
                    query,
                    (alias, team, league, sport) =>
                    {
                        alias.Team = team;
                        alias.Team.League = league;
                        alias.Team.League.Sport = sport;
                        return alias;
                    },
                    new
                    {
                        teamId = teamId
                    }
                );

                return result;
            }
        }

        /// <inheritdoc/>
        public Task<TeamAlias> UpdateAsync(TeamAlias entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}