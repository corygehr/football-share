using FootballShare.Entities.Betting;
using FootballShare.Entities.League;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Defines the <see cref="WeekEvent"/> repository interface
    /// </summary>
    public interface IWeekEventRepository : IRepository<WeekEvent>
    {
        /// <summary>
        /// Retrieves all <see cref="WeekEvent"/> objects for a specified <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="SeasonWeek"/> <see cref="WeekEvent"/> objects</returns>
        Task<IEnumerable<WeekEvent>> GetWeekEventsAsync(string weekId, CancellationToken cancellationToken = default);
    }
}
