using Dapper;
using FootballShare.Entities.User;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of the <see cref="ISiteRoleRepository"/> interface
    /// </summary>
    public class SqlSiteRoleRepository : ISiteRoleRepository
    {
        /// <summary>
        /// Database connection provider
        /// </summary>
        private IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSiteRoleRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection provider</param>
        public SqlSiteRoleRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task AddRoleMemberAsync(string userId, string roleName, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            // Get target role ID
            SiteRole targetRole = await this.GetByNameAsync(roleName, cancellationToken);

            if(targetRole != null)
            {
                string query = @"INSERT INTO [dbo].[SiteUserRoles] (
                                   [SiteUserId],
                                   [SiteRoleId],
                                   [WhenCreated]
                                 )
                                 VALUES (
                                   @userId,
                                   @roleId,
                                   CURRENT_TIMESTAMP
                                 )";

                using (var connection = this._connectionFactory.CreateConnection())
                {
                    await connection.ExecuteAsync(query,
                        new
                        {
                            roleId = targetRole.Id,
                            userId = userId
                        }
                    );
                }
            }
        }

        public async Task<SiteRole> CreateAsync(SiteRole role, CancellationToken cancellationToken = default)
        {
            // Check for required parameters
            if(role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            string query = $@"INSERT INTO [dbo].[SiteRoles](
                                [Name],
                                [NormalizedName]
                              )
                              VALUES (
                                @{nameof(SiteRole.Name)},
                                @{nameof(SiteRole.NormalizedName)}
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[SiteRoles]
                              WHERE [Id] = (CAST(SCOPE_IDENTITY() AS INT));
                            ";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteRole>(query, role);
            }
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            // Check required parameters
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[SiteRoles]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    id = entityId
                });
            }
        }

        public async Task DeleteAsync(SiteRole role, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(role.Id.ToString(), cancellationToken);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<IEnumerable<SiteRole>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[SiteRoles]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SiteRole>(query);
            }
        }

        public async Task<SiteRole> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            // Check for required parameters
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 * 
                              FROM [dbo].[SiteRoles]
                              WHERE [Id] = @roleId";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SiteRole>(
                    query,
                    new { roleId = entityId }
                );
            }
        }

        public async Task<SiteRole> GetAsync(SiteRole entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<SiteRole> GetByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(normalizedRoleName))
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            string query = $@"SELECT TOP 1 * 
                              FROM [dbo].[SiteRoles]
                              WHERE [NormalizedRoleName] = @normalizedRoleName";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SiteRole>(
                    query,
                    new { normalizedRoleName = normalizedRoleName }
                );
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(SiteRole role, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public async Task<IEnumerable<SiteRole>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT [ur].*
                              FROM [dbo].[SiteUserRoles] [ur]
                              INNER JOIN [dbo].[SiteRoles] [sr]
                                ON [ur].[SiteRoleId] = [sr].[Id]
                              WHERE [ur].[SiteUserId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SiteRole>(
                    query,
                    new
                    {
                        id = userId
                    }
                );
            }
        }

        public async Task<IEnumerable<SiteUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            string query = $@"SELECT [su].*
                              FROM [dbo].[SiteUsers] [su]
                              INNER JOIN [dbo].[SiteRoles] [sr]
                                ON [su].[Id] = [sr].[SiteUserId]
                              WHERE [sr].[Name] = @roleName";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SiteUser>(
                    query,
                    new
                    {
                        roleName = roleName
                    }
                );
            }
        }

        public async Task RemoveRoleMemberAsync(string userId, string roleName, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            // Get Role
            SiteRole targetRole = await this.GetByNameAsync(roleName, cancellationToken);

            if(targetRole != null)
            {
                string query = $@"DELETE FROM [dbo].[SiteRoles]
                                  WHERE [UserId] = @userId
                                  AND [RoleId] = @roleId";

                using (var connection = this._connectionFactory.CreateConnection())
                {
                    await connection.ExecuteAsync(
                        query,
                        new
                        {
                            roleId = targetRole.Id,
                            userId = userId
                        });
                }
            }
        }

        public Task SetNormalizedRoleNameAsync(SiteRole role, string normalizedName, CancellationToken cancellationToken = default)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(SiteRole role, string roleName, CancellationToken cancellationToken = default)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<SiteRole> UpdateAsync(SiteRole entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserInRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            // Get Role ID
            SiteRole role = await this.GetByNameAsync(roleName, cancellationToken);

            if(role != null)
            {
                string query = $@"SELECT COUNT(*)
                                  FROM [dbo].[SiteRoles]
                                  WHERE [SiteUserId] = @userId
                                  AND [Id] = @roleId";

                using (var connection = this._connectionFactory.CreateConnection())
                {
                    int roleCount = await connection.ExecuteScalarAsync<int>(
                        query,
                        new
                        {
                            userId = userId,
                            roleId = role.Id
                        }
                    );

                    return roleCount > 0;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
