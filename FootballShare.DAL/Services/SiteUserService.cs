using FootballShare.DAL.Repositories;
using FootballShare.Entities.Users;
using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="ISiteUserService"/> implementation
    /// </summary>
    public class SiteUserService : ISiteUserService
    {
        /// <summary>
        /// <see cref="SiteRole"/> repository
        /// </summary>
        private readonly ISiteRoleRepository _roleRepo;
        /// <summary>
        /// <see cref="SiteUserLoginProvider"/> repository
        /// </summary>
        private readonly ISiteUserLoginProviderRepository _userLoginProviderRepo;
        /// <summary>
        /// <see cref="SiteUser"/> repository
        /// </summary>
        private readonly ISiteUserRepository _userRepo;

        /// <summary>
        /// Creates a new <see cref="SiteUserService"/> instance
        /// </summary>
        /// <param name="userLoginProviderRepo"><see cref="SiteUserLoginProvider"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        public SiteUserService(ISiteRoleRepository roleRepo, ISiteUserLoginProviderRepository userLoginProviderRepo, ISiteUserRepository userRepo)
        {
            this._roleRepo = roleRepo;
            this._userLoginProviderRepo = userLoginProviderRepo;
            this._userRepo = userRepo;
        }

        public async Task AddLoginAsync(SiteUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            // Build model to create
            SiteUserLoginProvider providerInfo = new SiteUserLoginProvider
            {
                ExternalLoginId = Guid.NewGuid(),
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            };

            await this._userLoginProviderRepo.CreateAsync(providerInfo, cancellationToken);
        }

        public async Task AddToRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken = default)
        {
            await this._roleRepo.AddRoleMemberAsync(user.Id, roleName, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            try
            {
                await this._userRepo.CreateAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch(Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> DeleteAsync(SiteUser user, CancellationToken cancellationToken = default)
        {
            try
            {
                await this._userRepo.DeleteAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch(Exception)
            {
                return IdentityResult.Failed();
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<SiteUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await this._userRepo.GetByEmailAsync(normalizedEmail, cancellationToken);
        }

        public async Task<SiteUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await this._userRepo.GetAsync(userId, cancellationToken);
        }

        public async Task<SiteUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return await this._userRepo.GetByLoginProviderAsync(
                loginProvider,
                providerKey,
                cancellationToken
            );
        }

        public async Task<SiteUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await this._userRepo.GetByUserNameAsync(normalizedUserName, cancellationToken);
        }

        public Task<string> GetEmailAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(SiteUser user, CancellationToken cancellationToken)
        {
            // Get login providers
            IEnumerable<SiteUserLoginProvider> providers = await this._userLoginProviderRepo
                .GetAllForUserAsync(user.Id, cancellationToken);

            if(providers != null)
            {
                return providers
                    .Select(p => new UserLoginInfo(p.LoginProvider, p.ProviderKey, p.ProviderDisplayName))
                    .ToList();
            }
            else
            {
                return null;
            }
        }

        public Task<string> GetNormalizedEmailAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(SiteUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public async Task<IList<string>> GetRolesAsync(SiteUser user, CancellationToken cancellationToken)
        {
            // Get roles for user
            IEnumerable<SiteRole> userRoles = await this._roleRepo
                .GetUserRolesAsync(user.Id, cancellationToken);

            // Interface only wants the Role name(s)
            return userRoles.Select(r => r.Name).ToList();
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
            IEnumerable<SiteUser> users = await this._roleRepo.GetUsersInRoleAsync(roleName, cancellationToken);
            return users.ToList();
        }

        public async Task<bool> IsInRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken)
        {
            return await this._roleRepo.UserInRoleAsync(user.Id, roleName, cancellationToken);
        }

        public async Task RemoveFromRoleAsync(SiteUser user, string roleName, CancellationToken cancellationToken)
        {
            await this._roleRepo.RemoveRoleMemberAsync(user.Id, roleName, cancellationToken);
        }

        public async Task RemoveLoginAsync(SiteUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            await this._userLoginProviderRepo.DeleteAsync(
                new SiteUserLoginProvider
                {
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey,
                    UserId = user.Id
                },
                cancellationToken);
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

        public Task SetUserNameAsync(SiteUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(SiteUser user, CancellationToken cancellationToken)
        {
            try
            {
                await this._userRepo.UpdateAsync(user, cancellationToken);
                return IdentityResult.Success;
            }
            catch(Exception)
            {
                return IdentityResult.Failed();
            }
        }
    }
}
