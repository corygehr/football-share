using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// <see cref="Wager"/> repository interface
    /// </summary>
    public interface IWagerRepository : IRepository<Wager>
    {
        /// <summary>
        /// Retrieves a collection of <see cref="Wager"/> objects for a given 
        /// <see cref="SiteUser"/> during a <see cref="Season"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> FindBySeasonUserAndPoolAsync(int poolId, string seasonId, string userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a collection of <see cref="Wager"/> objects for a given user on a specific week
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> FindByWeekAndUserAsync(string weekId, Guid userId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a collection of <see cref="Wager"/> objects for a given user on a specific week
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> FindByWeekUserAndPoolAsync(int poolId, string weekId, Guid userId, CancellationToken cancellationToken = default);
    }
}
