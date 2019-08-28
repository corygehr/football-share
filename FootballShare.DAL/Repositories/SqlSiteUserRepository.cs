using Dapper;
using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of <see cref="ISiteUserRepository"/>
    /// </summary>
    public class SqlSiteUserRepository : ISiteUserRepository
    {
        /// <summary>
        /// Database connection provider
        /// </summary>
        private IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSiteUserRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection provider</param>
        public SqlSiteUserRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task AddLoginAsync(SiteUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            // Check required inputs
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            if(login == null)
            {
                throw new ArgumentNullException("login");
            }

            string query = $@"INSERT INTO [dbo].[SiteUserLoginProviders] (
                                [ExternalLoginId],
                                [UserId],
                                [LoginProvider],
                                [ProviderKey],
                                [ProviderDisplayName],
                                [WhenRegistered]
                            )
                            VALUES (
                                @externalLoginId,
                                @userId,
                                @loginProvider,
                                @providerKey,
                                @providerDisplayName,
                                CURRENT_TIMESTAMP
                            )";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    externalLoginId = Guid.NewGuid(),
                    userId = user.Id,
                    loginProvider = login.LoginProvider,
                    providerKey = login.ProviderKey,
                    providerDisplayName = login.ProviderDisplayName,
                    whenRegistered = DateTimeOffset.UtcNow
                });
            }
        }

        public async Task AddToRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken = default)
        {
            // Check for required parameters
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }

            if(String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException("roleName");
            }

            string getRoleIdQuery = @"SELECT TOP 1
                                        [Id]
                                      FROM [dbo].[SiteRoles]
                                      WHERE [NormalizedName] = @roleName";

            string assignRoleQuery = @"INSERT INTO [dbo].[UserRoles] (
                                        [UserId],
                                        [RoleId]
                                       )
                                       VALUES (
                                        @userId,
                                        @roleId
                                       )";


            using(var connection = this._connectionFactory.CreateConnection())
            {
                var roleId = await connection.ExecuteScalarAsync<int?>(
                    getRoleIdQuery,
                    new
                    {
                        roleName = roleName
                    });

                if(roleId.HasValue)
                {
                    await connection.ExecuteAsync(
                        assignRoleQuery,
                        new
                        {
                            roleId = roleId,
                            userId = user.Id
                        }
                    );
                }
            }
        }

        public async Task<IdentityResult> CreateAsync(SiteUser newEntity, CancellationToken cancellationToken = default)
        {
            // Check for required parameters
            if(newEntity == null)
            {
                throw new ArgumentException("newEntity");
            }

            string query = $@"INSERT INTO [dbo].[SiteUsers] (
                                [Id],
                                [UserName],
                                [NormalizedUserName],
                                [Email],
                                [NormalizedEmail],
                                [EmailConfirmed],
                                [WhenRegistered],
                                [WhenUpdated]
                            )
                            VALUES (
                                @{nameof(SiteUser.Id)},
                                @{nameof(SiteUser.UserName)},
                                @{nameof(SiteUser.NormalizedUserName)},
                                @{nameof(SiteUser.Email)},
                                @{nameof(SiteUser.NormalizedEmail)},
                                @{nameof(SiteUser.EmailConfirmed)},
                                CURRENT_TIMESTAMP,
                                CURRENT_TIMESTAMP
                            )";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                // Create new unique user identifier
                newEntity.Id = Guid.NewGuid();
                await connection.ExecuteAsync(query, newEntity);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            // Check for required parameters
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }

            string query = $@"DELETE FROM [dbo].[SiteUsers]
                              WHERE [Id] = @{nameof(SiteUser.Id)}";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, user);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<SiteUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT TOP 1
                                *
                            FROM [dbo].[SiteUsers]
                            WHERE [NormalizedEmail] = @{nameof(SiteUser.NormalizedEmail)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SiteUser>(query, new { normalizedEmail });
            }
        }

        public async Task<SiteUser> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT TOP 1
                                *
                            FROM [dbo].[SiteUsers]
                            WHERE [Id] = @{nameof(SiteUser.Id)}";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(
                    query, new { id }
                );
            }
        }

        public async Task<SiteUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            // Check required parameters
            if(String.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException("loginProvider");
            }

            if(String.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException("providerKey");
            }

            string query = $@"SELECT TOP 1 [u].*
                              FROM [dbo].[SiteUsers] [u]
                              INNER JOIN [dbo].[SiteUserLoginProviders] [slp]
                                ON [u].[Id] = [slp].[UserId]
                              WHERE [slp].[LoginProvider] = @loginProvider 
                              AND [slp].[ProviderKey] = @providerKey";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(query, new
                {
                    loginProvider = loginProvider,
                    providerKey = providerKey
                });
            }
        }

        public async Task<SiteUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT TOP 1
                                *
                            FROM [dbo].[SiteUsers]
                            WHERE [NormalizedUserName] = @{nameof(SiteUser.NormalizedUserName)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(
                    query, new { normalizedUserName }
                );
            }
        }

        public Task<string> GetEmailAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            // Check required inputs
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            string query = $@"SELECT *
                              FROM [dbo].[SiteUserLoginProviders]
                              WHERE [UserId] = @{nameof(SiteUser.Id)}";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                IEnumerable<SiteUserLoginProvider> rawResult = await connection
                    .QueryAsync<SiteUserLoginProvider>(query, user);

                // Cast return object into expected results
                List<UserLoginInfo> result = rawResult
                    .Select(r => new UserLoginInfo(r.LoginProvider, r.ProviderKey, r.ProviderDisplayName))
                    .ToList();

                return result;
            }
        }

        public Task<string> GetNormalizedEmailAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT [sr].[Name]
                              FROM [dbo].[UserRoles] [ur]
                              INNER JOIN [dbo].[SiteRoles] [sr]
                                ON [ur].[RoleId] = [sr].[Id]
                              WHERE [ur].[UserId] = @{nameof(SiteUser.Id)}";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                var userRoles = await connection.QueryAsync<string>(query, user);

                return userRoles.ToList();
            }
        }

        public Task<string> GetUserIdAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<SiteUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
        {
            string getRoleIdQuery = $@"SELECT TOP 1 [Id]
                                       FROM [dbo].[SiteRoles]
                                       WHERE [NormalizedName] = @roleName";

            string getUsersQuery = $@"SELECT [su].*
                                      FROM [dbo].[SiteUsers]
                                      WHERE [su].[Id] IN (
                                        SELECT [UserId]
                                        FROM [dbo].[UserRoles]
                                        WHERE [RoleId] = @roleId
                                      )";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                var roleId = await connection.ExecuteScalarAsync(
                    getRoleIdQuery,
                    new
                    {
                        roleName = roleName
                    }
                );
                var users = await connection.QueryAsync<SiteUser>(
                    getUsersQuery,
                    new
                    {
                        roleId = roleId
                    });

                return users.ToList();
            }
        }

        public Task<bool> HasPasswordAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        public async Task<bool> IsInRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken = default)
        {
            string getRoleIdQuery = $@"SELECT TOP 1
                                        [Id]
                                       FROM [dbo].[SiteRoles]
                                       WHERE [NormalizedName] = @roleName";

            string getUserInRoleQuery = $@"SELECT COUNT(*)
                                           FROM [dbo].[UserRoles]
                                           WHERE [UserId] = @userId
                                           AND [RoleId] = @roleId";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                var roleId = await connection.ExecuteScalarAsync<int?>(
                    getRoleIdQuery,
                    new
                    {
                        roleName = roleName.ToUpper()
                    }
                );

                if(roleId.HasValue)
                {
                    var roleCount = await connection.ExecuteScalarAsync<int>(
                        getUserInRoleQuery,
                        new
                        {
                            roleId = roleId,
                            userId = user.Id
                        }
                    );

                    return roleCount > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task RemoveFromRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken = default)
        {
            string getRoleIdQuery = $@"SELECT TOP 1
                                        [Id]
                                       FROM [dbo].[SiteRoles]
                                       WHERE [NormalizedName] = @roleName";

            string removeUserRoleQuery = $@"DELETE FROM [dbo].[SiteRoles]
                                            WHERE [UserId] = @userId
                                            AND [RoleId] = @roleId";
                              
            using(var connection = this._connectionFactory.CreateConnection())
            {
                var roleId = await connection.ExecuteScalarAsync<int?>(getRoleIdQuery, roleName);

                if(roleId.HasValue)
                {
                    await connection.ExecuteAsync(
                        removeUserRoleQuery,
                        new
                        {
                            roleId = roleId,
                            userId = user.Id
                        });
                }
            }
        }

        public async Task RemoveLoginAsync(SiteUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if(user == null)
            {
                throw new ArgumentNullException("user");
            }

            if(String.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException("loginProvider");
            }

            if(String.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException("providerKey");
            }

            string query = $@"DELETE FROM [dbo].[SiteUserLoginProviders]
                              WHERE [UserId] = @userId
                              AND [LoginProvider] = @loginProvider
                              AND [ProviderKey] = @providerKey";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    userId = user.Id,
                    loginProvider = loginProvider,
                    providerKey = providerKey
                });
            }
        }

        public Task SetEmailAsync(SiteUser user, string email, CancellationToken cancellationToken = default)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(SiteUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(SiteUser user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(SiteUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(SiteUser user, string passwordHash, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(SiteUser user, string userName, CancellationToken cancellationToken = default)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(SiteUser entity, CancellationToken cancellationToken = default)
        {
            string query = $@"UPDATE [dbo].[SiteUsers]
                              SET [FullName] = @{nameof(SiteUser.FullName)},
                                  [DisplayName] = @{nameof(SiteUser.DisplayName)},
                                  [Email] = @{nameof(SiteUser.Email)},
                                  [NormalizedEmail] = @{nameof(SiteUser.NormalizedEmail)},
                                  [EmailConfirmed] = @{nameof(SiteUser.EmailConfirmed)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(SiteUser.Id)}";
                
            using(var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }

            return IdentityResult.Success;
        }
    }
}