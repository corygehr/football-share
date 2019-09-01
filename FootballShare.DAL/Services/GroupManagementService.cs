using FootballShare.DAL.Repositories;
using FootballShare.Entities.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="BettingGroup"/> management service implementation
    /// </summary>
    public class GroupManagementService : IGroupManagementService
    {
        /// <summary>
        /// <see cref="BettingGroup"/> repository
        /// </summary>
        private readonly IBettingGroupRepository _groupRepo;
        /// <summary>
        /// <see cref="SiteUser"/> repository
        /// </summary>
        private readonly ISiteUserRepository _userRepo;

        /// <summary>
        /// Creates a new <see cref="GroupManagementService"/> instance
        /// </summary>
        /// <param name="groupRepo"><see cref="BettingGroup"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        public GroupManagementService(IBettingGroupRepository groupRepo, ISiteUserRepository userRepo)
        {
            this._groupRepo = groupRepo;
            this._userRepo = userRepo;
        }

        public Task<IEnumerable<BettingGroupMember>> GetGroupMembersAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
