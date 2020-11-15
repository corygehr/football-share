using FootballShare.DAL.Repositories;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="SportsLeague"/> service class
    /// </summary>
    public class SportsLeagueService : ISportsLeagueService
    {
        /// <summary>
        /// <see cref="SportsLeague"/> repository
        /// </summary>
        private readonly ISportsLeagueRepository _leagueRepo;
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
        /// <see cref="TeamAlias"/> repository
        /// </summary>
        private readonly ITeamAliasRepository _teamAliasRepo;
        /// <summary>
        /// <see cref="WeekEvent"/> repository
        /// </summary>
        private readonly IWeekEventRepository _weekEventRepo;

        /// <summary>
        /// Creates a new <see cref="SportsLeagueService"/> instance
        /// </summary>
        /// <param name="leagueRepo"><see cref="SportsLeague"/> repository</param>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        /// <param name="seasonWeekRepo"><see cref="SeasonWeek"/> repository</param>
        /// <param name="spreadRepo"><see cref="Spread"/> repository</param>
        /// <param name="teamRepo"><see cref="Team"/> repository</param>
        /// <param name="teamAliasRepo"><see cref="TeamAlias"/> repository</param>
        /// <param name="weekEventRepo"><see cref="WeekEvent"/> repository</param>
        public SportsLeagueService(ISportsLeagueRepository leagueRepo, ISeasonRepository seasonRepo, ISeasonWeekRepository seasonWeekRepo, ISpreadRepository spreadRepo, ITeamRepository teamRepo, ITeamAliasRepository teamAliasRepo, IWeekEventRepository weekEventRepo)
        {
            this._leagueRepo = leagueRepo;
            this._seasonRepo = seasonRepo;
            this._seasonWeekRepo = seasonWeekRepo;
            this._spreadRepo = spreadRepo;
            this._teamRepo = teamRepo;
            this._teamAliasRepo = teamAliasRepo;
            this._weekEventRepo = weekEventRepo;
        }

        /// <inheritdoc/>
        public async Task<WeekEvent> CreateWeekEventAsync(WeekEvent newEvent, CancellationToken cancellationToken = default)
        {
            // Insert the new WeekEvent
            WeekEvent createdEvent = await this._weekEventRepo.CreateAsync(newEvent, cancellationToken);

            if(createdEvent != null)
            {
                // Create the Spread associated with this new event
                Spread newSpread = new Spread
                {
                    WeekEventId = createdEvent.Id
                };

                Spread createdSpread = await this._spreadRepo.CreateAsync(newSpread, cancellationToken);

                if(createdSpread == null)
                {
                    // Rollback created event
                    await this._weekEventRepo.DeleteAsync(createdEvent, cancellationToken);
                    throw new Exception("Failed to create Spread for new WeekEvent. Operation cancelled and rolled back.");
                }
            }

            return createdEvent;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetAllCurrentSeasonsAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<SportsLeague> GetLeagueAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            return await this._leagueRepo.GetAsync(leagueId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Season> GetLeagueCurrentSeasonAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetCurrentLeagueSeasonAsync(leagueId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<SeasonWeek> GetLeagueCurrentSeasonWeekAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            Season currentSeason = await this._seasonRepo.GetCurrentLeagueSeasonAsync(leagueId.ToString(), cancellationToken);
            return await this._seasonWeekRepo.GetCurrentSeasonWeekAsync(currentSeason.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<SeasonWeek> GetLeaguePreviousWeekAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            // Get the current season
            Season currentSeason = await this.GetLeagueCurrentSeasonAsync(leagueId, cancellationToken);

            // Get the previous week
            return await this._seasonWeekRepo.GetPreviousSeasonWeekAsync(currentSeason.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Season> GetSeasonAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetAsync(seasonId, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<WeekEvent>> GetSeasonEventsAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<IEnumerable<SeasonWeek>> GetSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<Team> GetTeamAsync(string teamId, CancellationToken cancellationToken = default)
        {
            return await this._teamRepo.GetAsync(teamId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Team> GetTeamByNameAsync(string teamName, CancellationToken cancellationToken = default)
        {
            Team result = await this._teamRepo.GetByNameAsync(teamName, cancellationToken);

            // Attempt lookup assuming provided team name is simply an alias
            if(result == null)
            {
                IEnumerable<TeamAlias> aliasResult = await this._teamAliasRepo.GetByAliasAsync(teamName, cancellationToken);

                // Should only be one alias match
                if(aliasResult.Count() == 1)
                {
                    result = aliasResult.First().Team;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<WeekEvent> GetWeekEventAsync(int eventId, CancellationToken cancellationToken = default)
        {
            return await this._weekEventRepo.GetAsync(eventId.ToString(), cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<WeekEvent> GetWeekEventByWeekAndTeamsAsync(string weekId, string awayTeamId, string homeTeamId, CancellationToken cancellationToken = default)
        {
            return await this._weekEventRepo.GetWeekEventByWeekAndTeamsAsync(weekId, awayTeamId, homeTeamId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task UpdateEventScoreAsync(int eventId, int awayScore, int homeScore, CancellationToken cancellationToken = default)
        {
            // Get the WeekEvent targeted by this invocation
            WeekEvent targetEvent = await this._weekEventRepo.GetAsync(eventId.ToString(), cancellationToken);

            if(targetEvent != null)
            {
                // Update object and commit
                targetEvent.AwayScore = awayScore;
                targetEvent.HomeScore = homeScore;

                await this._weekEventRepo.UpdateAsync(targetEvent, cancellationToken);
            }
        }

        /// <inheritdoc/>
        public async Task UpsertWeekEventSpreadAsync(Spread spread, CancellationToken cancellationToken = default)
        {
            await this._spreadRepo.UpsertAsync(spread, cancellationToken);
        }
    }
}