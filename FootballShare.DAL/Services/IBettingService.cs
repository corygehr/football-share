using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

using System;
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
        /// Gets the current <see cref="SeasonWeek"/> for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current <see cref="SeasonWeek"/> for the specified <see cref="Season"/></returns>
        Task<SeasonWeek> GetCurrentSeasonWeekAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="Pool"/> <see cref="Wager"/> objects for a specific <see cref="Season"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetPoolWagersForSeasonAsync(int poolId, string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="Pool"/> <see cref="Wager"/> objects for a specific <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetPoolWagersForWeekAsync(int poolId, string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the previous <see cref="SeasonWeek"/> for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="SeasonWeek"/> object or null</returns>
        Task<SeasonWeek> GetPreviousSeasonWeekAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the previous <see cref="SeasonWeek"/> objects for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects</returns>
        Task<IEnumerable<SeasonWeek>> GetPreviousSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the schedule for an entire <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects or null if <see cref="Season"/> not found</returns>
        Task<IEnumerable<SeasonWeek>> GetSeasonScheduleAsync(int seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a specific <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="seasonWeekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="SeasonWeek"/></returns>
        Task<SeasonWeek> GetSeasonWeekAsync(string seasonWeekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the <see cref="Spread"/> for the provided <see cref="WeekEvent"/>
        /// </summary>
        /// <param name="eventId"><see cref="WeekEvent"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Spread"/> for provided <see cref="WeekEvent"/></returns>
        Task<Spread> GetSpreadForEventAsync(int eventId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all unresolved <see cref="Wager"/> objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetUnresolvedWagersAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="SiteUser"/> <see cref="Wager"/> objects for a specific <see cref="Season"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetUserWagersForSeasonAsync(Guid userId, string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="SiteUser"/> <see cref="Wager"/> objects for a specific <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> ID</param>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects, or null if not found</returns>
        Task<IEnumerable<Wager>> GetUserWagersForWeekByPoolAsync(Guid userId, string weekId, int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a specific <see cref="Wager"/> object
        /// </summary>
        /// <param name="id"><see cref="Wager"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="Wager"/> or null if not found</returns>
        Task<Wager> GetWagerAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a specific <see cref="WeekEvent"/>
        /// </summary>
        /// <param name="eventId"><see cref="WeekEvent"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="WeekEvent"/></returns>
        Task<WeekEvent> GetWeekEventAsync(int eventId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a collection of <see cref="WeekEvent"/> objects for a given <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="WeekEvent"/> objects</returns>
        Task<IEnumerable<WeekEvent>> GetWeekEventsAsync(string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves the spreads for the specified <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Spread"/> objects or null if <see cref="SeasonWeek"/> not found</returns>
        Task<IEnumerable<Spread>> GetWeekSpreadsAsync(string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="Wager"/> objects created for a specific <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Wager"/> objects</returns>
        Task<IEnumerable<Wager>> GetWeekWagersAsync(string weekId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Pays out a <see cref="Wager"/>
        /// </summary>
        /// <param name="source">Source <see cref="Wager"/></param>
        /// <param name="amount">Amount granted</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task PayoutWagerAsync(Wager source, decimal amount, CancellationToken cancellationToken = default);
        /// <summary>
        /// Places a <see cref="Wager"/>
        /// </summary>
        /// <param name="wager"><see cref="Wager"/> to place</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task PlaceWagerAsync(Wager wager, CancellationToken cancellationToken = default);
        /// <summary>
        /// Removes a <see cref="Wager"/>
        /// </summary>
        /// <param name="wagerId"><see cref="Wager"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveWagerAsync(int wagerId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates a <see cref="Wager"/>
        /// </summary>
        /// <param name="wager"><see cref="Wager"/> to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateWagerAsync(Wager wager, CancellationToken cancellationToken = default);
    }
}
