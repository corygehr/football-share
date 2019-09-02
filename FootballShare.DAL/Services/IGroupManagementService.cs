using FootballShare.Entities.Group;
using FootballShare.Entities.User;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="BettingGroup"/> management service
    /// </summary>
    public interface IGroupManagementService : IService
    {
        /// <summary>
        /// Adds a new <see cref="SiteUser"/> to a <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="user"><see cref="SiteUser"/> to add</param>
        /// <param name="group">Target <see cref="BettingGroup""/></param>
        /// <param name="asAdmin">Add <see cref="SiteUser"/> as Administrator</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddGroupMemberAsync(SiteUser user, BettingGroup group, bool asAdmin = false, CancellationToken cancellationToken = default);
        /// <summary>
        /// Creates a new <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="group">New <see cref="BettingGroup"/></param>
        /// <param name="userId">Creating <see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created <see cref="BettingGroup"/></returns>
        Task<BettingGroup> CreateNewGroupAsync(BettingGroup group, string userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a specific <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="groupId"><see cref="BettingGroup"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="BettingGroup"/></returns>
        Task<BettingGroup> GetGroupAsync(int groupId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all members of the specified <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="id"><see cref="BettingGroup"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="BettingGroup"/> or null if not found</returns>
        Task<IEnumerable<BettingGroupMember>> GetGroupMembersAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="BettingGroup"/> objects a <see cref="SiteUser"/> belongs to
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="BettingGroup"/> objects</returns>
        Task<IEnumerable<BettingGroup>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
