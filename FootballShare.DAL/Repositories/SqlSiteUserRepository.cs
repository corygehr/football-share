using Dapper;
using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        /// SQL Server connection string
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Creates a new <see cref="SqlSiteUserRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Connection factory</param>
        public SqlSiteUserRepository(IConfiguration siteConfig)
        {
            this._connectionString = siteConfig.GetConnectionString("DefaultConnection");
        }

        public async Task AddToRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken)
        {
            string getRoleIdQuery = @"SELECT 
                                        [Id]
                                      FROM [dbo].[SiteRoles]
                                      WHERE [NormalizedName] = @roleName
                                      LIMIT 1";

            string assignRoleQuery = @"INSERT INTO [dbo].[UserRoles] (
                                        [UserId],
                                        [RoleId]
                                       )
                                       VALUES (
                                        @userId,
                                        @roleId
                                       )";


            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
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
            string query = $@"INSERT INTO [dbo].[SiteUsers] (
                                [UserName],
                                [NormalizedUserName],
                                [Email],
                                [NormalizedEmail],
                                [EmailConfirmed],
                                [PasswordHash],
                                [PhoneNumber],
                                [PhoneNumberConfirmed],
                                [TwoFactorEnabled], 
                                [WhenRegistered]
                            )
                            VALUES (
                                @{nameof(SiteUser.UserName)},
                                @{nameof(SiteUser.NormalizedUserName)},
                                @{nameof(SiteUser.Email)},
                                @{nameof(SiteUser.NormalizedEmail)},
                                @{nameof(SiteUser.EmailConfirmed)},
                                @{nameof(SiteUser.PasswordHash)},
                                @{nameof(SiteUser.PhoneNumber)},
                                @{nameof(SiteUser.PhoneNumberConfirmed)},
                                @{nameof(SiteUser.TwoFactorEnabled)},
                                CURRENT_TIMESTAMP
                            );
                            # Get new SiteUser's ID
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                newEntity.Id = await connection.QuerySingleAsync<int>(query, newEntity);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            string query = $@"DELETE FROM [dbo].[SiteUsers]
                              WHERE [Id] = @{nameof(SiteUser.Id)}
                              LIMIT 1";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(query, user);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<SiteUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            string query = $@"SELECT
                                *
                            FROM [dbo].[SiteUsers]
                            WHERE [NormalizedEmail] = @{nameof(SiteUser.NormalizedEmail)}
                            LIMIT 1";

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<SiteUser>(query, new { normalizedEmail });
            }
        }

        public async Task<SiteUser> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                *
                            FROM [dbo].[SiteUsers]
                            WHERE [Id] = @{nameof(SiteUser.Id)}
                            LIMIT 1";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(
                    query, new { id }
                );
            }
        }

        public async Task<SiteUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            string query = $@"SELECT
                                *
                            FROM [dbo].[SiteUsers]
                            WHERE [NormalizedUserName] = @{nameof(SiteUser.NormalizedUserName)}
                            LIMIT 1";

            using (var connection = new SqlConnection(this._connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(
                    query, new { normalizedUserName }
                );
            }
        }

        public Task<string> GetEmailAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task<IList<string>> GetRolesAsync(SiteUser user, CancellationToken cancellationToken)
        {
            string query = $@"SELECT [sr].[Name]
                              FROM [dbo].[UserRoles] [ur]
                              INNER JOIN [dbo].[SiteRoles] [sr]
                                ON [ur].[RoleId] = [sr].[Id]
                              WHERE [ur].[UserId] = @{nameof(SiteUser.Id)}";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var userRoles = await connection.QueryAsync<string>(query, user);

                return userRoles.ToList();
            }
        }

        public Task<bool> GetTwoFactorEnabledAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<SiteUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            string getRoleIdQuery = $@"SELECT [Id]
                                       FROM [dbo].[SiteRoles]
                                       WHERE [NormalizedName] = @roleName
                                       LIMIT 1";

            string getUsersQuery = $@"SELECT [su].*
                                      FROM [dbo].[SiteUsers]
                                      WHERE [su].[Id] IN (
                                        SELECT [UserId]
                                        FROM [dbo].[UserRoles]
                                        WHERE [RoleId] = @roleId
                                      )";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
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

        public Task<bool> HasPasswordAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public async Task<bool> IsInRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken)
        {
            string getRoleIdQuery = $@"SELECT 
                                        [Id]
                                       FROM [dbo].[SiteRoles]
                                       WHERE [NormalizedName] = @roleName
                                       LIMIT 1";

            string getUserInRoleQuery = $@"SELECT COUNT(*)
                                           FROM [dbo].[UserRoles]
                                           WHERE [UserId] = @userId
                                           AND [RoleId] = @roleId
                                           LIMIT 1";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
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

        public async Task RemoveFromRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken)
        {
            string getRoleIdQuery = $@"SELECT
                                        [Id]
                                       FROM [dbo].[SiteRoles]
                                       WHERE [NormalizedName] = @roleName
                                       LIMIT 1";

            string removeUserRoleQuery = $@"DELETE FROM [dbo].[SiteRoles]
                                            WHERE [UserId] = @userId
                                            AND [RoleId] = @roleId";
                              
            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
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

        public Task SetEmailAsync(SiteUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(SiteUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(SiteUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(SiteUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(SiteUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(SiteUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(SiteUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(SiteUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(SiteUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(SiteUser entity, CancellationToken cancellationToken = default)
        {
            string query = $@"UPDATE [dbo].[SiteUsers]
                              SET [FullName] = @{nameof(SiteUser.FullName)},
                                  [Email] = @{nameof(SiteUser.Email)},
                                  [NormalizedEmail] = @{nameof(SiteUser.NormalizedEmail)},
                                  [EmailConfimed] = @{nameof(SiteUser.EmailConfirmed)},
                                  [PasswordHash] = @{nameof(SiteUser.PasswordHash)},
                                  [PhoneNumber] = @{nameof(SiteUser.PhoneNumber)},
                                  [PhoneNumberConfirmed] = @{nameof(SiteUser.PhoneNumberConfirmed)},
                                  [TwoFactorEnabled] = @{nameof(SiteUser.TwoFactorEnabled)}
                              WHERE [Id] = @{nameof(SiteUser.Id)}
                              LIMIT 1";
                
            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(query, entity);
            }

            return IdentityResult.Success;
        }
    }
}