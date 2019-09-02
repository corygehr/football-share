using FootballShare.DAL.Repositories;
using FootballShare.Entities.Group;
using FootballShare.Entities.User;

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
        /// <see cref="BettingGroupMember"/> repository
        /// </summary>
        private readonly IBettingGroupMemberRepository _groupMemberRepo;
        /// <summary>
        /// <see cref="SiteUser"/> repository
        /// </summary>
        private readonly ISiteUserRepository _userRepo;

        /// <summary>
        /// Creates a new <see cref="GroupManagementService"/> instance
        /// </summary>
        /// <param name="groupRepo"><see cref="BettingGroup"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        public GroupManagementService(IBettingGroupRepository groupRepo, IBettingGroupMemberRepository groupMemberRepo, ISiteUserRepository userRepo)
        {
            this._groupRepo = groupRepo;
            this._groupMemberRepo = groupMemberRepo;
            this._userRepo = userRepo;
        }

        public async Task AddGroupMemberAsync(SiteUser user, BettingGroup group, bool asAdmin = false, CancellationToken cancellationToken = default)
        {
            await this._groupMemberRepo.CreateAsync(
                new BettingGroupMember
                {
                    BettingGroupId = group.Id,
                    IsAdmin = asAdmin,
                    SiteUserId = user.Id
                },
                cancellationToken);
        }

        public async Task<BettingGroup> CreateNewGroupAsync(BettingGroup group, string userId, CancellationToken cancellationToken = default)
        {
            SiteUser adminUser = await this._userRepo.GetAsync(userId, cancellationToken);
            BettingGroup newGroup = await this._groupRepo.CreateAsync(group, cancellationToken);
            // Add creating user as an administrator
            await this.AddGroupMemberAsync(adminUser, newGroup, true, cancellationToken);

            return newGroup;
        }

        public async Task<BettingGroup> GetGroupAsync(int groupId, CancellationToken cancellationToken = default)
        {
            return await this._groupRepo.GetAsync(groupId.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<BettingGroupMember>> GetGroupMembersAsync(int id, CancellationToken cancellationToken = default)
        {
            BettingGroup group = await this._groupRepo.GetAsync(id.ToString(), cancellationToken);
            IEnumerable<BettingGroupMember> members = await this._groupRepo.GetBettingGroupMembersAsync(id, cancellationToken);

            // Get User objects for each member
            BettingGroupMember[] fullMembers = new BettingGroupMember[members.Count()];

            for(int i=0; i<members.Count(); i++)
            {
                BettingGroupMember member = members.ElementAt(i);
                member.User = await this._userRepo.GetAsync(member.SiteUserId.ToString(), cancellationToken);
                member.BettingGroup = group;
                fullMembers[i] = member;
            }

            return fullMembers.ToList();
        }

        public async Task<IEnumerable<BettingGroup>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await this._groupRepo.SearchByMemberUserIdAsync(userId, cancellationToken);
        }
    }
}
