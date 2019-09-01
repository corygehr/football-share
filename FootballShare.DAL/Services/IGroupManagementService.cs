using FootballShare.Entities.Group;

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
        /// Retrieves all members of the specified <see cref="BettingGroup"/>
        /// </summary>
        /// <param name="id"><see cref="BettingGroup"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="BettingGroup"/> or null if not found</returns>
        Task<IEnumerable<BettingGroupMember>> GetGroupMembersAsync(int id, CancellationToken cancellationToken = default);
    }
}
