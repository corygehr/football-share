using FootballShare.Entities.Betting;
using FootballShare.Entities.League;

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
        /// Retrieves a collection of <see cref="Wager"/> objects for a given user on a specific week
        /// </summary>
        Task<IEnumerable<Wager>> FindByWeekAndUser(SeasonWeek weekEvent, string userId, CancellationToken cancellationToken = default);
    }
}
