using FootballShare.DAL.Repositories;
using FootballShare.Entities.Users;
using Microsoft.AspNetCore.Identity;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    public class SiteRoleService : ISiteRoleService
    {
        /// <summary>
        /// <see cref="SiteRole"/> repository
        /// </summary>
        private readonly ISiteRoleRepository _roleRepo;

        /// <summary>
        /// Creates a new <see cref="SiteRoleService"/> instance
        /// </summary>
        /// <param name="roleRepo"><see cref="SiteRole"/> repository</param>
        public SiteRoleService(ISiteRoleRepository roleRepo)
        {
            this._roleRepo = roleRepo;
        }

        public async Task<IdentityResult> CreateAsync(SiteRole role, CancellationToken cancellationToken)
        {
            try
            {
                await this._roleRepo.CreateAsync(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(SiteRole role, CancellationToken cancellationToken)
        {
            try
            {
                await this._roleRepo.DeleteAsync(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<SiteRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await this._roleRepo.GetAsync(roleId, cancellationToken);
        }

        public async Task<SiteRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await this._roleRepo.GetByNameAsync(normalizedRoleName, cancellationToken);
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
            try
            {
                await this._roleRepo.UpdateAsync(role, cancellationToken);
                return IdentityResult.Success;
            }
            catch(Exception)
            {
                return IdentityResult.Failed();
            }
        }
    }
}
