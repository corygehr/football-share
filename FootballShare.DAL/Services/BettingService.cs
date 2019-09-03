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
        /// <see cref="Season"/> repository
        /// </summary>
        private readonly ISeasonRepository _seasonRepo;
        /// <summary>
        /// <see cref="SeasonWeek"/> repository
        /// </summary>
        private readonly ISeasonWeekRepository _seasonWeekRepo;
        /// <summary>
        /// <see cref="Spread"/> repository
        /// </summary>
        private readonly ISpreadRepository _spreadRepo;
        /// <summary>
        /// <see cref="Team"/> repository
        /// </summary>
        private readonly ITeamRepository _teamRepo;
        /// <summary>
        /// <see cref="Wager"/> repository
        /// </summary>
        private readonly IWagerRepository _wagerRepo;
        /// <summary>
        /// <see cref="WeekEvent"/> repository
        /// </summary>
        private readonly IWeekEventRepository _weekEventRepo;

        /// <summary>
        /// Creates a new <see cref="BettingService"/> instance
        /// </summary>
        /// <param name="eventRepo"><see cref="WeekEvent"/> repository</param>
        /// <param name="poolRepo"><see cref="Pool"/> repository</param>
        /// <param name="poolMemberRepo"><see cref="PoolMember"/> repository</param>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        /// <param name="seasonWeekRepo"><see cref="SeasonWeek"/> repository</param>
        /// <param name="teamRepo"><see cref="Team"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        /// <param name="wagerRepo"><see cref="Wager"/> repository</param>
        /// <param name="weekEventRepo"><see cref="WeekEvent"/> repository</param>
        public BettingService(IWeekEventRepository eventRepo, IPoolRepository poolRepo, 
            IPoolMemberRepository poolMemberRepo, ISeasonRepository seasonRepo, ISeasonWeekRepository seasonWeekRepo, 
            ISpreadRepository spreadRepo, ITeamRepository teamRepo, ISiteUserRepository userRepo,
            IWagerRepository wagerRepo, IWeekEventRepository weekEventRepo)
        {
            this._eventRepo = eventRepo;
            this._poolRepo = poolRepo;
            this._poolMemberRepo = poolMemberRepo;
            this._seasonRepo = seasonRepo;
            this._seasonWeekRepo = seasonWeekRepo;
            this._spreadRepo = spreadRepo;
            this._teamRepo = teamRepo;
            this._wagerRepo = wagerRepo;
            this._weekEventRepo = weekEventRepo;
        }

        public async Task<SeasonWeek> GetCurrentSeasonWeekAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            return await this._seasonWeekRepo.GetCurrentSeasonWeekAsync(seasonId, cancellationToken);
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

        public async Task<IEnumerable<SeasonWeek>> GetPreviousSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            return await this._seasonWeekRepo.GetPreviousSeasonWeeksAsync(seasonId, cancellationToken);
        }

        public Task<IEnumerable<SeasonWeek>> GetSeasonScheduleAsync(int seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<SeasonWeek> GetSeasonWeekAsync(string seasonWeekId, CancellationToken cancellationToken = default)
        {
            SeasonWeek seasonWeek = await this._seasonWeekRepo.GetAsync(seasonWeekId, cancellationToken);
            seasonWeek.Season = await this._seasonRepo.GetAsync(seasonWeek.SeasonId, cancellationToken);
            return seasonWeek;
        }

        public async Task<Spread> GetSpreadForEventAsync(int eventId, CancellationToken cancellationToken = default)
        {
            Spread spread = await this._spreadRepo.GetByWeekEventAsync(eventId.ToString(), cancellationToken);
            spread.Event = await this.GetWeekEventAsync(eventId, cancellationToken);
            return spread;
        }

        public Task<IEnumerable<Wager>> GetUserWagersForSeasonAsync(Guid userId, string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wager>> GetUserWagersForWeekAsync(Guid userId, int weekId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wager>> GetUserWagersForWeekByPoolAsync(Guid userId, int weekId, int poolId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Wager> GetWagerAsync(string id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<WeekEvent> GetWeekEventAsync(int eventId, CancellationToken cancellationToken = default)
        {
            WeekEvent weekEvent = await this._weekEventRepo.GetAsync(eventId.ToString(), cancellationToken);
            weekEvent.Week = await this.GetSeasonWeekAsync(weekEvent.SeasonWeekId.ToString(), cancellationToken);
            weekEvent.AwayTeam = await this._teamRepo.GetAsync(weekEvent.AwayTeamId, cancellationToken);
            weekEvent.HomeTeam = await this._teamRepo.GetAsync(weekEvent.HomeTeamId, cancellationToken);
            return weekEvent;
        }

        public async Task<IEnumerable<Spread>> GetWeekSpreads(string weekId, CancellationToken cancellationToken = default)
        {
            SeasonWeek week = await this._seasonWeekRepo.GetAsync(weekId, cancellationToken);

            // Get events for the specified week
            IEnumerable<WeekEvent> events = await this._weekEventRepo.GetWeekEventsAsync(weekId, cancellationToken);
            List<Spread> spreads = new List<Spread>();

            foreach(WeekEvent weekEvent in events)
            {
                Spread eventSpread = await this.GetSpreadForEventAsync(weekEvent.Id, cancellationToken);
                WeekEvent fullEvent = await this.GetWeekEventAsync(weekEvent.Id, cancellationToken);
                eventSpread.Event = fullEvent;
                spreads.Add(eventSpread);
            }

            return spreads;
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
