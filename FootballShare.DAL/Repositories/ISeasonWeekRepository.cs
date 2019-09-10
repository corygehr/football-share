using FootballShare.Entities.Leagues;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="SeasonWeek"/> interface
    /// </summary>
    public interface ISeasonWeekRepository : IRepository<SeasonWeek>
    {
        /// <summary>
        /// Gets all <see cref="SeasonWeek"/> objects for the requested <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects</returns>
        Task<IEnumerable<SeasonWeek>> GetAllForSeasonAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the next <see cref="SeasonWeek"/> for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Next <see cref="SeasonWeek"/> or null if none</returns>
        Task<SeasonWeek> GetCurrentSeasonWeekAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="SeasonWeek"/> objects which have already passed for the specified <see cref="Season"/> 
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects</returns>
        Task<IEnumerable<SeasonWeek>> GetPreviousSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default);
    }
}
