using FootballShare.Entities.League;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="SportsLeague"/> Service interface
    /// </summary>
    public interface ISportsLeagueService : IService
    {
        /// <summary>
        /// Gets all current <see cref="Season"/> objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Season"/> objects</returns>
        Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the specified <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId"><see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="SportsLeague"/></returns>
        Task<SportsLeague> GetLeagueAsync(string leagueId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the current <see cref="Season"/> for the specified <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId">Requested <see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current <see cref="SportsLeague"/> <see cref="Season"/></returns>
        Task<Season> GetLeagueCurrentSeasonAsync(string leagueId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Season"/> object</returns>
        Task<Season> GetSeasonAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="WeekEvent"/> objects for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId">Target <see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="WeekEvent"/> objects</returns>
        Task<IEnumerable<WeekEvent>> GetSeasonEventsAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="SeasonWeek"/> objects for a specific <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId">Target <see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects</returns>
        Task<IEnumerable<SeasonWeek>> GetSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default);
    }
}
