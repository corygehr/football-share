using Dapper;
using FootballShare.Entities.User;

using System;
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
        IDbConnectionFactory _connectionFactory;

        /// <summary>
        /// Creates a new <see cref="SqlSiteUserRepository"/> instance
        /// </summary>
        /// <param name="connectionFactory">Connection factory</param>
        public SqlSiteUserRepository(IDbConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public SiteUser CreateAsync(SiteUser newEntity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<SiteUser> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT
                                [Id],
                                [UserName],
                                [NormalizedUserName],
                                [FullName],
                                [Email]
                                [NormalizedEmail],
                                [EmailConfirmed],
                                [PasswordHash],
                                [PhoneNumber],
                                [PhoneNumberConfirmed],
                                [TwoFactorEnabled],
                                [AccessFailedCount],
                                [LockoutEnabled],
                                [LockoutEnd],
                                [WhenRegistered],
                                [WhenUpdated]
                            FROM [dbo].[SiteUsers]
                            WHERE [Id] = @{nameof(id)}
                            LIMIT 1";

            using(var connection = this._connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<SiteUser>(
                    query, new { id = id }
                );
            }
        }

        public Task UpdateAsync(SiteUser entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}