using FootballShare.DAL.Exceptions;
using FootballShare.DAL.Repositories;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
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
        /// <see cref="LedgerEntry"/> repository
        /// </summary>
        private readonly ILedgerEntryRepository _ledgerRepo;
        /// <summary>
        /// <see cref="Pool"/> repository
        /// </summary>
        private readonly IPoolRepository _poolRepo;
        /// <summary>
        /// <see cref="PoolMember"/> repository
        /// </summary>
        private readonly IPoolMemberRepository _poolMemberRepo;
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
        /// <param name="ledgerRepo"><see cref="LedgerEntry"/> repository</param>
        /// <param name="poolRepo"><see cref="Pool"/> repository</param>
        /// <param name="poolMemberRepo"><see cref="PoolMember"/> repository</param>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        /// <param name="seasonWeekRepo"><see cref="SeasonWeek"/> repository</param>
        /// <param name="teamRepo"><see cref="Team"/> repository</param>
        /// <param name="userRepo"><see cref="SiteUser"/> repository</param>
        /// <param name="wagerRepo"><see cref="Wager"/> repository</param>
        public BettingService(ILedgerEntryRepository ledgerRepo, IPoolRepository poolRepo, 
            IPoolMemberRepository poolMemberRepo, ISeasonRepository seasonRepo, ISeasonWeekRepository seasonWeekRepo, 
            ISpreadRepository spreadRepo, ITeamRepository teamRepo, ISiteUserRepository userRepo,
            IWagerRepository wagerRepo, IWeekEventRepository weekEventRepo)
        {
            this._ledgerRepo = ledgerRepo;
            this._poolRepo = poolRepo;
            this._poolMemberRepo = poolMemberRepo;
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

        public async Task<IEnumerable<Wager>> GetPoolWagersForWeekAsync(int poolId, string weekId, CancellationToken cancellationToken = default)
        {
            return await this._wagerRepo.GetForPoolByWeekAsync(poolId, weekId, cancellationToken);
        }

        public async Task<SeasonWeek> GetPreviousSeasonWeekAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            return await this._seasonWeekRepo.GetPreviousSeasonWeekAsync(seasonId, cancellationToken);
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
            return await this._seasonWeekRepo.GetAsync(seasonWeekId, cancellationToken);
        }

        public async Task<Spread> GetSpreadForEventAsync(int eventId, CancellationToken cancellationToken = default)
        {
            return await this._spreadRepo.GetByWeekEventAsync(eventId, cancellationToken);
        }

        public async Task<IEnumerable<Wager>> GetUnresolvedWagersAsync(CancellationToken cancellationToken = default)
        {
            return await this._wagerRepo.GetUnresolvedWagersAsync(cancellationToken);
        }

        public Task<IEnumerable<Wager>> GetUserWagersForSeasonAsync(Guid userId, string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Wager>> GetUserWagersForWeekAsync(Guid userId, string weekId, CancellationToken cancellationToken = default)
        {
            return await this._wagerRepo.GetForUserByWeekAsync(userId, weekId, cancellationToken);
        }

        public async Task<IEnumerable<Wager>> GetUserWagersForWeekByPoolAsync(Guid userId, string weekId, int poolId, CancellationToken cancellationToken = default)
        {
            return await this._wagerRepo.GetForUserByPoolAndWeekAsync(userId, poolId, weekId, cancellationToken);
        }

        public async Task<Wager> GetWagerAsync(string id, CancellationToken cancellationToken = default)
        {
            return await this._wagerRepo.GetAsync(id, cancellationToken);
        }

        public async Task<WeekEvent> GetWeekEventAsync(int eventId, CancellationToken cancellationToken = default)
        {
            return await this._weekEventRepo.GetAsync(eventId.ToString());
        }

        public async Task<IEnumerable<WeekEvent>> GetWeekEventsAsync(string weekId, CancellationToken cancellationToken = default)
        {
            return await this._weekEventRepo.GetWeekEventsAsync(weekId, cancellationToken);
        }

        public async Task<IEnumerable<Spread>> GetWeekSpreadsAsync(string weekId, CancellationToken cancellationToken = default)
        {
            // Get events for the specified week
            IEnumerable<WeekEvent> events = await this._weekEventRepo.GetWeekEventsAsync(weekId, cancellationToken);
            List<Spread> spreads = new List<Spread>();

            foreach(WeekEvent weekEvent in events)
            {
                Spread eventSpread = await this.GetSpreadForEventAsync(weekEvent.Id, cancellationToken);
                eventSpread.Event = weekEvent;
                spreads.Add(eventSpread);
            }

            return spreads;
        }

        public Task<IEnumerable<Wager>> GetWeekWagersAsync(string weekId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task PayoutWagerAsync(Wager source, decimal amount, CancellationToken cancellationToken = default)
        {
            // Get user data
            PoolMember member = await this._poolMemberRepo
                .GetMembershipAsync(source.SiteUserId, source.PoolId, cancellationToken);

            // Create ledger entry
            LedgerEntry transactionRecord = new LedgerEntry
            {
                Description = $"{source.Result} (${source.Amount} on {source.SelectedTeamId} @ {source.SelectedTeamSpread})",
                NewBalance = member.Balance + amount,
                PoolId = source.PoolId,
                SiteUserId = member.SiteUserId,
                StartingBalance = member.Balance,
                TransactionAmount = amount,
                WagerId = source.Id
            };

            // Update member balance
            member.Balance = transactionRecord.NewBalance;

            await this._ledgerRepo.CreateAsync(transactionRecord, cancellationToken);
            await this._poolMemberRepo.UpdateAsync(member, cancellationToken);
        }

        public async Task PlaceWagerAsync(Wager wager, CancellationToken cancellationToken = default)
        {
            // Get PoolMember
            PoolMember bettor = await this._poolMemberRepo.GetMembershipAsync(
                wager.SiteUserId,
                wager.PoolId,
                cancellationToken);

            if(bettor == null)
            {
                throw new ArgumentNullException(nameof(bettor));
            }
            else
            {
                bettor.Pool = await this._poolRepo.GetAsync(bettor.PoolId.ToString(), cancellationToken);
            }

            WeekEvent weekEvent = await this.GetWeekEventAsync(wager.WeekEventId, cancellationToken);

            if(weekEvent == null)
            {
                throw new ArgumentNullException(nameof(weekEvent));
            }

            // Check valididty of wager options
            if(weekEvent.HomeTeamId != wager.SelectedTeamId && weekEvent.AwayTeamId != wager.SelectedTeamId)
            {
                throw new ArgumentException("Chosen team is not involved in the selected event.");
            }

            // Get user wagers for validation
            IEnumerable<Wager> wagers = await this.GetUserWagersForWeekByPoolAsync(
                bettor.SiteUserId,
                weekEvent.SeasonWeekId,
                wager.PoolId
            );

            if(wagers.Count() >= bettor.Pool.WagersPerWeek)
            {
                throw new MaxBetsPlacedForWeekException(bettor.Pool.WagersPerWeek);
            }

            // Check if user has enough funds
            if (wager.Amount > bettor.Balance)
            {
                throw new NotEnoughFundsException(wager.Amount, bettor.Balance);
            }

            // Place bet, add ledger entry, update account
            Wager newWager = await this._wagerRepo.CreateAsync(wager);
            LedgerEntry ledger = new LedgerEntry
            {
                Description = $"Placed ${wager.Amount} on {wager.SelectedTeamId} ({wager.SelectedTeamSpread}).",
                NewBalance = (bettor.Balance - wager.Amount),
                PoolId = bettor.PoolId,
                SiteUserId = bettor.SiteUserId,
                StartingBalance = bettor.Balance,
                TransactionAmount = -(wager.Amount),
                WagerId = newWager.Id,
            };
            await this._ledgerRepo.CreateAsync(ledger);

            bettor.Balance = ledger.NewBalance;
            await this._poolMemberRepo.UpdateAsync(bettor);
        }

        public async Task RemoveWagerAsync(int wagerId, CancellationToken cancellationToken = default)
        {
            // Get wager
            Wager toRemove = await this._wagerRepo.GetAsync(wagerId.ToString(), cancellationToken);

            if(toRemove == null)
            {
                throw new ArgumentException("Invalid Wager ID.");
            }

            // Get PoolMember
            PoolMember user = await this._poolMemberRepo.GetMembershipAsync(
                toRemove.SiteUserId,
                toRemove.PoolId,
                cancellationToken
            );

            // Roll-back wager
            await this._wagerRepo.DeleteAsync(toRemove, cancellationToken);

            // Place bet, add ledger entry, update account
            LedgerEntry ledger = new LedgerEntry
            {
                Description = $"Cancelled ${toRemove.Amount} on {toRemove.SelectedTeamId} ({toRemove.SelectedTeamSpread}).",
                NewBalance = (user.Balance + toRemove.Amount),
                PoolId = toRemove.PoolId,
                SiteUserId = toRemove.SiteUserId,
                StartingBalance = user.Balance,
                TransactionAmount = toRemove.Amount,
                WagerId = toRemove.Id,
            };
            await this._ledgerRepo.CreateAsync(ledger);

            user.Balance = ledger.NewBalance;
            await this._poolMemberRepo.UpdateAsync(user);
        }

        public async Task UpdateWagerAsync(Wager wager, CancellationToken cancellationToken = default)
        {
            await this._wagerRepo.UpdateAsync(wager, cancellationToken);
        }
    }
}