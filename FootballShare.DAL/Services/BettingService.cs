using FootballShare.DAL.Repositories;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Group;
using FootballShare.Entities.League;
using FootballShare.Entities.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// Service implementation for <see cref="IBettingService"/>
    /// </summary>
    public class BettingService : IBettingService
    {
        /// <summary>
        /// <see cref="WeekEvent"/> repository
        /// </summary>
        private readonly IWeekEventRepository _eventRepo;
        /// <summary>
        /// <see cref="BettingGroup"/> repository
        /// </summary>
        private readonly IBettingGroupRepository _groupRepo;
        /// <summary>
        /// <see cref="SiteUser"/> repository
        /// </summary>
        private readonly ISiteUserRepository _userRepo;
        /// <summary>
        /// <see cref="Wager"/> repository
        /// </summary>
        private readonly IWagerRepository _wagerRepo;

        /// <summary>
        /// Creates a new <see cref="BettingService"/> instance
        /// </summary>
        /// <param name="eventRepo"><see cref="WeekEvent"/> repository</param>
        /// <param name="groupRepo"><see cref="BettingGroup"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        /// <param name="wagerRepo"><see cref="Wager"/> repository</param>
        public BettingService(IWeekEventRepository eventRepo, IBettingGroupRepository groupRepo, 
            ISiteUserRepository userRepo, IWagerRepository wagerRepo)
        {
            this._eventRepo = eventRepo;
            this._groupRepo = groupRepo;
            this._wagerRepo = wagerRepo;
        }

        public async Task<IEnumerable<Wager>> GetGroupWagersForSeasonAsync(int groupId, string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(seasonId);
            }

            // Get all users in selected group
            IEnumerable<BettingGroupMember> members = await this._groupRepo.GetBettingGroupMembersAsync(groupId, cancellationToken);

            if(members.Count() > 0)
            {
                // Aggregate users
                List<Wager> wagers = new List<Wager>();

                foreach(BettingGroupMember member in members)
                {
                    IEnumerable<Wager> userWagers = await this.GetUserWagersForSeasonAsync(member.SiteUserId, seasonId, cancellationToken);

                    if(userWagers != null)
                    {
                        wagers.AddRange(userWagers);
                    }
                }

                return wagers;
            }

            return null;
        }

        public async Task<IEnumerable<Wager>> GetGroupWagersForWeekAsync(int groupId, int weekId, CancellationToken cancellationToken = default)
        {
            // Get all users in selected group
            IEnumerable<BettingGroupMember> members = await this._groupRepo.GetBettingGroupMembersAsync(groupId, cancellationToken);

            if (members.Count() > 0)
            {
                // Aggregate users
                List<Wager> wagers = new List<Wager>();

                foreach (BettingGroupMember member in members)
                {
                    // Get user

                    IEnumerable<Wager> userWagers = await this.GetUserWagersForWeekAsync(member.SiteUserId, weekId, cancellationToken);

                    if (userWagers != null)
                    {
                        wagers.AddRange(userWagers);
                    }
                }

                return wagers;
            }

            return null;
        }

        public Task<IEnumerable<SeasonWeek>> GetSeasonScheduleAsync(int seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wager>> GetUserWagersForSeasonAsync(int userId, string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wager>> GetUserWagersForWeekAsync(int userId, int weekId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Wager> GetWagerAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Spread>> GetWeekSpreads(int weekId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task PlaceWagerAsync(Wager wager, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveWagerAsync(Wager wager, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
