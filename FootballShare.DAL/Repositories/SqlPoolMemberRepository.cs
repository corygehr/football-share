using Dapper;
using FootballShare.Entities.Leagues;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IPoolMemberRepository"/>
    /// </summary>
    public class SqlPoolMemberRepository : IPoolMemberRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;
        
        /// <summary>
        /// Creates a new <see cref="SqlPoolMemberRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory"></param>
        public SqlPoolMemberRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<PoolMember> CreateAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[PoolMembers] (
                                [PoolId],
                                [SiteUserId],
                                [IsAdmin],
                                [Balance],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(PoolMember.PoolId)},
                                @{nameof(PoolMember.SiteUserId)},
                                @{nameof(PoolMember.IsAdmin)},
                                @{nameof(PoolMember.Balance)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[PoolMembers]
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT));";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolMember>(query, entity);
            }
        }

        public async Task DeleteAsync(PoolMember entity, CancellationToken cancellationToken = default)
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

            string query = $@"DELETE FROM [dbo].[PoolMembers]
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

        public async Task<IEnumerable<PoolMember>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [pm].*,
                                [p].*,
                                [p_s].*,
                                [p_s_l].*,
                                [p_s_l_s].*,
                                [su].*
                              FROM [dbo].[PoolMembers] [pm]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [pm].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Seasons] [p_s]
                                ON [p].[SeasonId] = [p_s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [p_s_l]
                                ON [p_s].[SportsLeagueId] = [p_s_l].[Id]
                              INNER JOIN [dbo].[Sports] [p_s_l_s]
                                ON [p_s_l].[SportId] = [p_s_l_s].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [pm].[SiteUserId] = [su].[Id]";
            
            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<PoolMember, Pool, Season, SportsLeague, Sport, SiteUser, PoolMember>(
                    query,
                    (poolMember, pool, season, league, sport, siteUser) =>
                    {
                        poolMember.Pool = pool;
                        poolMember.Pool.Season = season;
                        poolMember.Pool.Season.League = league;
                        poolMember.Pool.Season.League.Sport = sport;
                        poolMember.User = siteUser;

                        return poolMember;
                    }
                );
            }
        }

        public async Task<PoolMember> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT
                                TOP 1 [pm].*,
                                [p].*,
                                [p_s].*,
                                [p_s_l].*,
                                [p_s_l_s].*,
                                [su].*
                              FROM [dbo].[PoolMembers] [pm]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [pm].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Seasons] [p_s]
                                ON [p].[SeasonId] = [p_s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [p_s_l]
                                ON [p_s].[SportsLeagueId] = [p_s_l].[Id]
                              INNER JOIN [dbo].[Sports] [p_s_l_s]
                                ON [p_s_l].[SportId] = [p_s_l_s].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [pm].[SiteUserId] = [su].[Id]
                              WHERE [pm].[Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<PoolMember> result = await connection.QueryAsync<PoolMember, Pool, Season, SportsLeague, Sport, SiteUser, PoolMember>(
                    query,
                    (poolMember, pool, season, league, sport, siteUser) =>
                    {
                        poolMember.Pool = pool;
                        poolMember.Pool.Season = season;
                        poolMember.Pool.Season.League = league;
                        poolMember.Pool.Season.League.Sport = sport;
                        poolMember.User = siteUser;

                        return poolMember;
                    },
                    new
                    {
                        id = entityId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<PoolMember> GetAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<PoolMember> GetMembershipAsync(Guid userId, int poolId, CancellationToken cancellationToken = default)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT
                                TOP 1 [pm].*,
                                [p].*,
                                [p_s].*,
                                [p_s_l].*,
                                [p_s_l_s].*,
                                [su].*
                              FROM [dbo].[PoolMembers] [pm]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [pm].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Seasons] [p_s]
                                ON [p].[SeasonId] = [p_s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [p_s_l]
                                ON [p_s].[SportsLeagueId] = [p_s_l].[Id]
                              INNER JOIN [dbo].[Sports] [p_s_l_s]
                                ON [p_s_l].[SportId] = [p_s_l_s].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [pm].[SiteUserId] = [su].[Id]
                              WHERE [pm].[SiteUserId] = @userId 
                              AND [pm].[PoolId] = @poolId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<PoolMember> result = await connection.QueryAsync<PoolMember, Pool, Season, SportsLeague, Sport, SiteUser, PoolMember>(
                    query,
                    (poolMember, pool, season, league, sport, siteUser) =>
                    {
                        poolMember.Pool = pool;
                        poolMember.Pool.Season = season;
                        poolMember.Pool.Season.League = league;
                        poolMember.Pool.Season.League.Sport = sport;
                        poolMember.User = siteUser;

                        return poolMember;
                    },
                    new
                    {
                        userId = userId,
                        poolId = poolId
                    }
                );

                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<PoolMember>> GetPoolMembersAsync(int poolId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [pm].*,
                                [p].*,
                                [p_s].*,
                                [p_s_l].*,
                                [p_s_l_s].*,
                                [su].*
                              FROM [dbo].[PoolMembers] [pm]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [pm].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Seasons] [p_s]
                                ON [p].[SeasonId] = [p_s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [p_s_l]
                                ON [p_s].[SportsLeagueId] = [p_s_l].[Id]
                              INNER JOIN [dbo].[Sports] [p_s_l_s]
                                ON [p_s_l].[SportId] = [p_s_l_s].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [pm].[SiteUserId] = [su].[Id]
                              WHERE [pm].[PoolId] = @id
                              ORDER BY [pm].[Balance] DESC";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<PoolMember, Pool, Season, SportsLeague, Sport, SiteUser, PoolMember>(
                    query,
                    (poolMember, pool, season, league, sport, siteUser) =>
                    {
                        poolMember.Pool = pool;
                        poolMember.Pool.Season = season;
                        poolMember.Pool.Season.League = league;
                        poolMember.Pool.Season.League.Sport = sport;
                        poolMember.User = siteUser;

                        return poolMember;
                    },
                    new
                    {
                        id = poolId
                    }
                );
            }
        }

        public async Task<IEnumerable<PoolMember>> GetUserMembershipsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT
                                [pm].*,
                                [p].*,
                                [p_s].*,
                                [p_s_l].*,
                                [p_s_l_s].*,
                                [su].*
                              FROM [dbo].[PoolMembers] [pm]
                              INNER JOIN [dbo].[Pools] [p]
                                ON [pm].[PoolId] = [p].[Id]
                              INNER JOIN [dbo].[Seasons] [p_s]
                                ON [p].[SeasonId] = [p_s].[Id]
                              INNER JOIN [dbo].[SportsLeagues] [p_s_l]
                                ON [p_s].[SportsLeagueId] = [p_s_l].[Id]
                              INNER JOIN [dbo].[Sports] [p_s_l_s]
                                ON [p_s_l].[SportId] = [p_s_l_s].[Id]
                              INNER JOIN [dbo].[SiteUsers] [su]
                                ON [pm].[SiteUserId] = [su].[Id]
                              WHERE [pm].[SiteUserId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<PoolMember, Pool, Season, SportsLeague, Sport, SiteUser, PoolMember>(
                    query,
                    (poolMember, pool, season, league, sport, siteUser) =>
                    {
                        poolMember.Pool = pool;
                        poolMember.Pool.Season = season;
                        poolMember.Pool.Season.League = league;
                        poolMember.Pool.Season.League.Sport = sport;
                        poolMember.User = siteUser;

                        return poolMember;
                    },
                    new
                    {
                        id = userId
                    }
                );
            }
        }

        public async Task<PoolMember> UpdateAsync(PoolMember entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[PoolMembers]
                              SET [IsAdmin] = @{nameof(PoolMember.IsAdmin)},
                                  [Balance] = @{nameof(PoolMember.Balance)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(PoolMember.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[PoolMembers]
                              WHERE [Id] = @{nameof(PoolMember.Id)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<PoolMember>(query, entity);
            }
        }
    }
}
