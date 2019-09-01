using Dapper;
using FootballShare.Entities.User;

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

        public async Task<SiteUser> CreateAsync(SiteUser newEntity, CancellationToken cancellationToken = default)
        {
            // Check for required parameters
            if(newEntity == null)
            {
                throw new ArgumentException(nameof(newEntity));
            }

            // Create new unique user identifier
            newEntity.Id = Guid.NewGuid();

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
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[SiteUsers]
                              WHERE [Id] = @id;";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteUser>(query, newEntity);
            }
        }

        public async Task DeleteAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            // Use overload
            await this.DeleteAsync(user.Id.ToString(), cancellationToken);
        }

        public async Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"DELETE FROM [dbo].[SiteUsers]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, new
                {
                    id = entityId
                });
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<IEnumerable<SiteUser>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT *
                              FROM [dbo].[SiteUsers]";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SiteUser>(query);
            }
        }

        public async Task<SiteUser> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SiteUsers]
                              WHERE [Id] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteUser>(query, new
                {
                    id = entityId
                });
            }
        }

        public async Task<SiteUser> GetAsync(SiteUser entity, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAsync(entity.Id.ToString(), cancellationToken);
        }

        public async Task<SiteUser> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(normalizedEmail))
            {
                throw new ArgumentNullException(nameof(normalizedEmail));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SiteUsers]
                              WHERE [NormalizedEmail] = @emailAddress";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SiteUser>(query, new
                {
                    emailAddress = normalizedEmail
                });
            }
        }

        public async Task<SiteUser> GetByLoginProviderAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            // Check required parameters
            if (String.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }

            if (String.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException(nameof(providerKey));
            }

            string query = $@"SELECT TOP 1 [u].*
                              FROM [dbo].[SiteUsers] [u]
                              INNER JOIN [dbo].[SiteUserLoginProviders] [slp]
                                ON [u].[Id] = [slp].[UserId]
                              WHERE [slp].[LoginProvider] = @loginProvider 
                              AND [slp].[ProviderKey] = @providerKey";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(query, new
                {
                    loginProvider = loginProvider,
                    providerKey = providerKey
                });
            }
        }

        public async Task<SiteUser> GetByProviderKeyAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }

            if (String.IsNullOrEmpty(providerKey))
            {
                throw new ArgumentNullException(nameof(providerKey));
            }

            string query = $@"SELECT TOP 1 [su].*
                              FROM [dbo].[SiteUsers] [su]
                              INNER JOIN [dbo].[SiteUserLoginProviders] [sp]
                                ON [su].[Id] = [sp].[UserId]
                              WHERE [sp].[LoginProvider] = @loginProvider
                              AND [sp].[ProviderKey] = @providerKey";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteUser>(
                    query,
                    new
                    {
                        loginProvider = loginProvider,
                        providerKey = providerKey
                    }
                );
            }
        }

        public async Task<SiteUser> GetByUserNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            if (String.IsNullOrEmpty(normalizedUserName))
            {
                throw new ArgumentNullException(nameof(normalizedUserName));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SiteUsers]
                              WHERE [NormalizedUserName] = @userName";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(
                    query,
                    new
                    {
                        userName = normalizedUserName
                    }
                );
            }
        }

        public async Task<SiteUser> UpdateAsync(SiteUser entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"UPDATE [dbo].[SiteUsers]
                              SET [FullName] = @{nameof(SiteUser.FullName)},
                                  [DisplayName] = @{nameof(SiteUser.DisplayName)},
                                  [Email] = @{nameof(SiteUser.Email)},
                                  [NormalizedEmail] = @{nameof(SiteUser.NormalizedEmail)},
                                  [EmailConfirmed] = @{nameof(SiteUser.EmailConfirmed)},
                                  [WhenUpdated] = CURRENT_TIMESTAMP
                              WHERE [Id] = @{nameof(SiteUser.Id)};
                              SELECT TOP 1 *
                              FROM [dbo].[SiteUsers]
                              WHERE [Id] = @{nameof(SiteUser.Id)};";
                
            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteUser>(query, entity);
            }
        }
    }
}