using FootballShare.Entities.Betting;
using FootballShare.Entities.League;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// Service for betting-related activities
    /// </summary>
    public interface IBettingService : IService
    {
        /// <summary>
        /// Retrieves all <see cref="BettingGroup"/> <see cref="Wager"/> objects for a specific <see cref="Season"/>
        /// </summary>
        /// <param name="groupId"><see cref="BettingGroup"/> ID</param>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetGroupWagersForSeasonAsync(int groupId, string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="BettingGroup"/> <see cref="Wager"/> objects for a specific <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="groupId"><see cref="BettingGroup"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetGroupWagersForWeekAsync(int groupId, int weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the schedule for an entire <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects or null if <see cref="Season"/> not found</returns>
        Task<IEnumerable<SeasonWeek>> GetSeasonScheduleAsync(int seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="SiteUser"/> <see cref="Wager"/> objects for a specific <see cref="Season"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetUserWagersForSeasonAsync(int userId, string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="SiteUser"/> <see cref="Wager"/> objects for a specific <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetUserWagersForWeekAsync(int userId, int weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a specific <see cref="Wager"/> object
        /// </summary>
        /// <param name="id"><see cref="Wager"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="Wager"/> or null if not found</returns>
        Task<Wager> GetWagerAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the spreads for the specified <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Spread"/> objects or null if <see cref="SeasonWeek"/> not found</returns>
        Task<IEnumerable<Spread>> GetWeekSpreads(int weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Places a <see cref="Wager"/>
        /// </summary>
        /// <param name="wager"><see cref="Wager"/> to place</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task PlaceWagerAsync(Wager wager, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes a <see cref="Wager"/>
        /// </summary>
        /// <param name="wager"><see cref="Wager"/> to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveWagerAsync(Wager wager, CancellationToken cancellationToken = default);
    }
}
