using FootballShare.Entities.Betting;

using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="Spread"/> repository interface
    /// </summary>
    public interface ISpreadRepository : IRepository<Spread>
    {
        /// <summary>
        /// Retrieves a <see cref="Spread"/> for a specific <see cref="WeekEvent"/>
        /// </summary>
        /// <param name="weekEventId"><see cref="WeekEvent"</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Spread"/> for specified <see cref="WeekEvent"/></returns>
        Task<Spread> GetByWeekEventAsync(string weekEventId, CancellationToken cancellationToken = default);
    }
}
