using FootballShare.Entities.Leagues;

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
        /// Retrieves the current <see cref="Season"/> for each <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Season"/> objects</returns>
        Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves the current <see cref="Season"/> for the specific <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId">Target <see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current <see cref="SportsLeague"/> <see cref="Season"/></returns>
        Task<Season> GetCurrentLeagueSeasonAsync(string leagueId, CancellationToken cancellationToken);
    }
}
