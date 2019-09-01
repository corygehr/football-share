using FootballShare.Entities.User;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// <see cref="SiteRole"/> base repository interface
    /// </summary>
    public interface ISiteRoleRepository : IRepository<SiteRole>
    {
        /// <summary>
        /// Adds the specified <see cref="SiteUser"/> to the requested <see cref="SiteRole"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID for <see cref="SiteRole"/> assignment</param>
        /// <param name="roleName"><see cref="SiteRole"/> name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddRoleMemberAsync(string userId, string roleName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a specific <see cref="SiteRole"/> by its name
        /// </summary>
        /// <param name="normalizedRoleName"><see cref="SiteRole"/> mormalized name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="SiteRole"/></returns>
        Task<SiteRole> GetByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="SiteRole"/> objects for a specific <see cref="SiteUser"/>
        /// </summary>
        /// <param name="userId">Target <see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SiteRole"/> objects</returns>
        Task<IEnumerable<SiteRole>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="SiteUser"/> objects with a given <see cref="SiteRole"/>
        /// </summary>
        /// <param name="roleName">Role name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SiteUser"/> objects</returns>
        Task<IEnumerable<SiteUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes the specified <see cref="SiteUser"/> from the requested <see cref="SiteRole"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID for <see cref="SiteRole"/> de-assignment</param>
        /// <param name="roleName"><see cref="SiteRole"/> name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveRoleMemberAsync(string userId, string roleName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Checks if the specified <see cref="SiteUser"/> is in the requested <see cref="SiteRole"/>
        /// </summary>
        /// <param name="userId">Target <see cref="SiteUser"/> ID</param>
        /// <param name="roleName">Target <see cref="SiteRole"/> name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if <see cref="SiteUser"/> is in requested <see cref="SiteRole"/>, false if not</returns>
        Task<bool> UserInRoleAsync(string userId, string roleName, CancellationToken cancellationToken = default);
    }
}
