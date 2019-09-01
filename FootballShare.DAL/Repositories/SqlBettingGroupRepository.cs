using Dapper;
using FootballShare.Entities.Group;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="IBettingGroupRepository"/>
    /// </summary>
    public class SqlBettingGroupRepository : IBettingGroupRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlBettingGroupRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlBettingGroupRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<BettingGroup> CreateAsync(BettingGroup newEntity, CancellationToken cancellationToken = default)
        {
            if (newEntity == null)
            {
                throw new ArgumentNullException("newEntity");
            }

            string query = $@"INSERT INTO [dbo].[BettingGroups] (
                                [Description],
                                [Name],
                                [WhenCreated],
                                [WhenUpdated]
                              )
                              VALUES (
                                @{nameof(BettingGroup.Description)},
                                @{nameof(BettingGroup.Name)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[BettingGroups]
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT));
                            ";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<BettingGroup>(query, newEntity);
            }
        }

        public async Task DeleteAsync(BettingGroup entity, CancellationToken cancellationToken = default)
        {
            // Use overloaded method
            await this.DeleteAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[BettingGroups]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    id = entityId
                });
            }
        }

        public async Task<IEnumerable<BettingGroup>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = @"SELECT *
                             FROM [dbo].[BettingGroups]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<BettingGroup>(query);
            }
        }

        public async Task<IEnumerable<BettingGroup>> GetAllPublicAsync(CancellationToken cancellationToken = default)
        {
            string query = @"SELECT * 
                             FROM [dbo].[BettingGroups]
                             WHERE [IsPublic] = 1";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<BettingGroup>(query);
            }
        }

        public async Task<BettingGroup> GetAsync(BettingGroup entity, CancellationToken cancellationToken = default)
        {
            // Use overloaded method
            return await this.GetAsync(entity.Id.ToString());
        }

        public async Task<BettingGroup> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[BettingGroups]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<BettingGroup>(query, new
                {
                    id = entityId
                });
            }
        }

        public async Task<IEnumerable<BettingGroupMember>> GetBettingGroupMembersAsync(int groupId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[BettingGroupMembers]
                              WHERE [BettingGroupId] = @groupId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<BettingGroupMember>(query, new
                {
                    groupId = groupId
                });
            }
        }

        public async Task<IEnumerable<BettingGroupPool>> GetBettingGroupPoolsAsync(int groupId, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[BettingGroupPools]
                              WHERE [BettingGroupId] = @groupId";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<BettingGroupPool>(query, new
                {
                    groupId = groupId
                });
            }
        }

        public async Task<IEnumerable<BettingGroup>> SearchByMemberUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            string query = $@"SELECT [bg].*
                              FROM [dbo].[BettingGroups] [bg]
                              INNER JOIN [dbo].[BettingGroupMembers] [bgm]
                                ON [bg].[Id] = [bgm].[BettingGroupId]
                              WHERE [bgm].[SiteUserId] = @userId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<BettingGroup>(query, new
                {
                    userId = userId
                });
            }
        }

        public async Task<IEnumerable<BettingGroup>> SearchByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            string query = $@"SELECT *
                              FROM [dbo].[BettingGroups]
                              WHERE [Name] LIKE @name";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<BettingGroup>(query, new
                {
                    name = name
                });
            }
        }

        public async Task<BettingGroup> UpdateAsync(BettingGroup entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            string query = $@"UPDATE [dbo].[BettingGroups]
                              SET [Name] = @{nameof(BettingGroup.Name)},
                                  [Description] = @{nameof(BettingGroup.Description)},
                                  [IsPublic] = @{nameof(BettingGroup.IsPublic)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(BettingGroup.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[BettingGroups]
                              WHERE [Id] = @{nameof(BettingGroup.Id)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<BettingGroup>(query, entity);
            }
        }
    }
}
