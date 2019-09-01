using FootballShare.Entities.League;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Defines the <see cref="SportsLeague"/> repository interface
    /// </summary>
    public interface ISportsLeagueRepository : IRepository<SportsLeague>
    {
        /// <summary>
        /// Retrieves all <see cref="SportsLeague"/> objects available
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SportsLeague"/> objects</returns>
        Task<IEnumerable<SportsLeague>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
