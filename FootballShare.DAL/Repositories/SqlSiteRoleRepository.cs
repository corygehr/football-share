using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// SQL Server implementation of the <see cref="ISiteRoleRepository"/> interface
    /// </summary>
    public class SqlSiteRoleRepository : ISiteRoleRepository
    {
        /// <summary>
        /// SQL Server connection string
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Creates a new <see cref="SqlSiteRoleRepository"/> instance
        /// </summary>
        /// <param name="siteConfig">Site configuration data</param>
        public SqlSiteRoleRepository(IConfiguration siteConfig)
        {
            this._connectionString = siteConfig.GetConnectionString("DefaultConnection");
        }

        public async Task<IdentityResult> CreateAsync(SiteRole role, CancellationToken cancellationToken = default)
        {
            string query = $@"INSERT INTO [dbo].[SiteRoles](
                                [Name],
                                [NormalizedName]
                              )
                              VALUES (
                                @{nameof(SiteRole.Name)},
                                @{nameof(SiteRole.NormalizedName)}
                              );
                            # Get created RoleId
                            SELECT CAST(SCOPE_IDENTITY() AS INT);
                            ";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                role.Id = await connection.QuerySingleAsync<int>(query, role);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(SiteRole role, CancellationToken cancellationToken = default)
        {
            string query = $@"DELETE FROM [dbo].[SiteRoles]
                              WHERE [Id] = @{nameof(SiteRole.Id)}
                              LIMIT 1";
            
            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(query, role);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<SiteRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            string query = $@"SELECT * 
                              FROM [dbo].[SiteRoles]
                              WHERE [Id] = @roleId
                              LIMIT 1";

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<SiteRole>(
                    query,
                    new { roleId = id }
                );
            }
        }

        public async Task<SiteRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            string query = $@"SELECT * 
                              FROM [dbo].[SiteRoles]
                              WHERE [NormalizedRoleName] = @roleName
                              LIMIT 1";

            using (var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<SiteRole>(
                    query,
                    new { roleName = normalizedRoleName }
                );
            }
        }

        public Task<string> GetNormalizedRoleNameAsync(SiteRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(SiteRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(SiteRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(SiteRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(SiteRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(SiteRole role, CancellationToken cancellationToken)
        {
            string query = $@"UPDATE [dbo].[SiteRoles]
                              SET [Name] = @{nameof(SiteRole.Name)},
                                  [NormalizedName] = @{nameof(SiteRole.NormalizedName)}
                              WHERE [Id] = @{nameof(SiteRole.Id)}
                              LIMIT 1";

            using(var connection = new SqlConnection(this._connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(query, role);
            }

            return IdentityResult.Success;
        }
    }
}
