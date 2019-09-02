using FootballShare.Entities.Betting;

using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="PoolUserBalance"/> interface
    /// </summary>
    public interface IPoolUserBalanceRepository : IRepository<PoolUserBalance>
    {
        /// <summary>
        /// Checks if the <see cref="SiteUser"/> has enough remaining funds
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> unique identifier</param>
        /// <param name="groupId"><see cref="BettingGroupPoolId"/> unique identifier</param>
        /// <param name="requiredBalance">Balance required (>)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if user meets minimum balance, False if not</returns>
        Task<bool> UserHasBalanceAsync(string userId, int groupId, double requiredBalance = 0, CancellationToken cancellationToken = default)
    }
}
