using Dapper;
using FootballShare.Entities.Users;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of the <see cref="ISiteUserLoginProviderRepository"/> interface
    /// </summary>
    public class SqlSiteUserLoginProviderRepository : ISiteUserLoginProviderRepository
    {
        /// <summary>
        /// Database connection factory
        /// </summary>
        private readonly IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSiteUserLoginProviderRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Database connection factory</param>
        public SqlSiteUserLoginProviderRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public async Task<SiteUserLoginProvider> CreateAsync(SiteUserLoginProvider entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"INSERT INTO [dbo].[SiteUserLoginProviders] (
                                [ExternalLoginId],
                                [UserId],
                                [LoginProvider],
                                [ProviderKey],
                                [ProviderDisplayName],
                                [WhenCreated]
                              )
                              VALUES (
                                @{nameof(SiteUserLoginProvider.ExternalLoginId)},
                                @{nameof(SiteUserLoginProvider.UserId)},
                                @{nameof(SiteUserLoginProvider.LoginProvider)},
                                @{nameof(SiteUserLoginProvider.ProviderKey)},
                                @{nameof(SiteUserLoginProvider.ProviderDisplayName)},
                                CURRENT_TIMESTAMP
                              );
                              SELECT TOP 1 *
                              FROM [dbo].[SiteUserLoginProviders]
                              WHERE [ExternalLoginId] = @{nameof(SiteUserLoginProvider.ExternalLoginId)}
                              AND [UserId] = @{nameof(SiteUserLoginProvider.UserId)};";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteUserLoginProvider>(query, entity);
            }
        }

        public async Task DeleteAsync(SiteUserLoginProvider entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"DELETE FROM [dbo].[SiteUserLoginProviders]
                              WHERE [LoginProvider] = @{nameof(SiteUserLoginProvider.LoginProvider)}
                              AND [ProviderKey] = @{nameof(SiteUserLoginProvider.ProviderKey)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(query, entity);
            }
        }

        public Task DeleteAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SiteUserLoginProvider>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<SiteUserLoginProvider>> GetAllForUserAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            // Use overload
            return await this.GetAllForUserAsync(user.Id, cancellationToken);
        }

        public async Task<IEnumerable<SiteUserLoginProvider>> GetAllForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string query = $@"SELECT *
                              FROM [dbo].[SiteUserLoginProviders]
                              WHERE [UserId] = @id";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<SiteUserLoginProvider>(
                    query,
                    new
                    {
                        id = userId
                    }
                );
            }
        }

        public Task<SiteUserLoginProvider> GetAsync(string entityId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SiteUserLoginProvider> GetAsync(SiteUserLoginProvider entity, CancellationToken cancellationToken = default)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            string query = $@"SELECT TOP 1 *
                              FROM [dbo].[SiteUserLoginProviders]
                              WHERE [UserId] = @{nameof(SiteUserLoginProvider.UserId)}
                              AND [LoginProvider] = @{nameof(SiteUserLoginProvider.LoginProvider)}
                              AND [ProviderKey] = @{nameof(SiteUserLoginProvider.ProviderKey)}";

            using (var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleAsync<SiteUserLoginProvider>(query, entity);
            }
        }

        public Task<SiteUserLoginProvider> UpdateAsync(SiteUserLoginProvider entity, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
