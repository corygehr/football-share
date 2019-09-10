using FootballShare.DAL.Repositories;
using FootballShare.Entities.Leagues;

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
        /// Creates a new <see cref="SportsLeagueService"/> instance
        /// </summary>
        /// <param name="leagueRepo"><see cref="SportsLeague"/> repository</param>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        public SportsLeagueService(ISportsLeagueRepository leagueRepo, ISeasonRepository seasonRepo)
        {
            this._leagueRepo = leagueRepo;
            this._seasonRepo = seasonRepo;
        }

        public async Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetAllCurrentSeasonsAsync(cancellationToken);
        }

        public async Task<SportsLeague> GetLeagueAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            return await this._leagueRepo.GetAsync(leagueId, cancellationToken);
        }

        public async Task<Season> GetLeagueCurrentSeasonAsync(string leagueId, CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetCurrentLeagueSeasonAsync(leagueId.ToString(), cancellationToken);
        }

        public async Task<Season> GetSeasonAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            return await this._seasonRepo.GetAsync(seasonId, cancellationToken);
        }

        public Task<IEnumerable<WeekEvent>> GetSeasonEventsAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SeasonWeek>> GetSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
