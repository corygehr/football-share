using FootballShare.DAL.Repositories;
using FootballShare.Entities.League;

using System;
using System.Collections.Generic;
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
        /// <see cref="Sport"/> repository
        /// </summary>
        private readonly ISportRepository _sportRepo;

        /// <summary>
        /// Creates a new <see cref="SportsLeagueService"/> instance
        /// </summary>
        /// <param name="leagueRepo"><see cref="SportsLeague"/> repository</param>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        /// <param name="sportRepo"><see cref="Sport"/> repository</param>
        public SportsLeagueService(ISportsLeagueRepository leagueRepo, ISeasonRepository seasonRepo, ISportRepository sportRepo)
        {
            this._leagueRepo = leagueRepo;
            this._seasonRepo = seasonRepo;
            this._sportRepo = sportRepo;
        }

        public async Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetAllCurrentSeasonsAsync(cancellationToken);
        }

        public async Task<SportsLeague> GetLeagueAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            SportsLeague league = await this._leagueRepo.GetAsync(leagueId, cancellationToken);

            if(league != null)
            {
                league.Sport = await this.GetSportAsync(league.SportId, cancellationToken);
            }

            return league;
        }

        public async Task<Season> GetLeagueCurrentSeasonAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            Season current = await this._seasonRepo.GetCurrentForLeagueAsync(leagueId.ToString(), cancellationToken);

            if(current != null)
            {
                current.League = await this.GetLeagueAsync(current.SportsLeagueId, cancellationToken);
            }

            return current;
        }

        public async Task<Season> GetSeasonAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            Season season = await this._seasonRepo.GetAsync(seasonId, cancellationToken);

            if(season != null)
            {
                season.League = await this.GetLeagueAsync(season.SportsLeagueId, cancellationToken);
            }

            return season;
        }

        public Task<IEnumerable<WeekEvent>> GetSeasonEventsAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SeasonWeek>> GetSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Sport> GetSportAsync(string sportId, CancellationToken cancellationToken = default)
        {
            return await this._sportRepo.GetAsync(sportId, cancellationToken);
        }
    }
}
