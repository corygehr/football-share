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
        /// Gets a collection of <see cref="Wager"/> objects for a <see cref="Pool"/> by the specified 
        /// <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetForPoolByWeekAsync(int poolId, string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of <see cref="Wager"/> objects created by a <see cref="SiteUser"/> for the specified 
        /// <see cref="Pool"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetForUserByPoolAsync(Guid userId, int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of <see cref="Wager"/> objects created by a <see cref="SiteUser"/> for the specified 
        /// <see cref="Pool"/> on a specified <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetForUserByPoolAndWeekAsync(Guid userId, int poolId, string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of <see cref="Wager"/> objects created by a <see cref="SiteUser"/> for the specified 
        /// <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetForUserByWeekAsync(Guid userId, string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of <see cref="Wager"/> objects which have not been resolved
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetUnresolvedWagersAsync(CancellationToken cancellationToken = default);
    }
}
