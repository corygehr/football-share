using FootballShare.DAL.Repositories;
using FootballShare.Entities.Betting;
using FootballShare.Entities.League;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

using System;
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
        /// <see cref="Pool"/> repository
        /// </summary>
        private readonly IPoolRepository _poolRepo;
        /// <summary>
        /// <see cref="PoolMember"/> repository
        /// </summary>
        private readonly IPoolMemberRepository _poolMemberRepo;
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
        /// <param name="poolRepo"><see cref="Pool"/> repository</param>
        /// <param name="poolMemberRepo"><see cref="PoolMember"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        /// <param name="wagerRepo"><see cref="Wager"/> repository</param>
        public BettingService(IWeekEventRepository eventRepo, IPoolRepository poolRepo, 
            IPoolMemberRepository poolMemberRepo,ISiteUserRepository userRepo, IWagerRepository wagerRepo)
        {
            this._eventRepo = eventRepo;
            this._poolRepo = poolRepo;
            this._poolMemberRepo = poolMemberRepo;
            this._wagerRepo = wagerRepo;
        }

        public async Task<IEnumerable<Wager>> GetPoolWagersForSeasonAsync(int poolId, string seasonId, CancellationToken cancellationToken = default)
        {
            if(String.IsNullOrEmpty(seasonId))
            {
                throw new ArgumentNullException(seasonId);
            }

            // Get all users in selected group
            IEnumerable<PoolMember> members = await this._poolMemberRepo.GetPoolMembersAsync(poolId, cancellationToken);

            if(members.Count() > 0)
            {
                // Aggregate users
                List<Wager> wagers = new List<Wager>();

                foreach(PoolMember member in members)
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

        public async Task<IEnumerable<Wager>> GetPoolWagersForWeekAsync(int poolId, int weekId, CancellationToken cancellationToken = default)
        {
            // Get all users in selected group
            IEnumerable<PoolMember> members = await this._poolMemberRepo
                .GetPoolMembersAsync(poolId, cancellationToken);

            if (members.Count() > 0)
            {
                // Aggregate users
                List<Wager> wagers = new List<Wager>();

                foreach (PoolMember member in members)
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

        public Task<IEnumerable<Wager>> GetUserWagersForSeasonAsync(Guid userId, string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wager>> GetUserWagersForWeekAsync(Guid userId, int weekId, CancellationToken cancellationToken = default)
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
