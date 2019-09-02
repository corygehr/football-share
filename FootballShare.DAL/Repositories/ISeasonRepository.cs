using FootballShare.Entities.League;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Defines the <see cref="Season"/> repository interface
    /// </summary>
    public interface ISeasonRepository : IRepository<Season>
    {
        /// <summary>
        /// Retrieves all <see cref="SeasonWeek"/> objects for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="season">Target <see cref="Season"/></param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects</returns>
        Task<IEnumerable<SeasonWeek>> GetAllWeeksAsync(Season season, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves the current <see cref="Season"/> for the specific <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId">Target <see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current <see cref="SportsLeague"/> <see cref="Season"/></returns>
        Task<Season> GetCurrentForLeagueAsync(string leagueId, CancellationToken cancellationToken);
    }
}
