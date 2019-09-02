using FootballShare.Entities.Pools;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="PoolMember"/> repository
    /// </summary>
    public interface IPoolMemberRepository : IRepository<PoolMember>
    {
        /// <summary>
        /// Gets all <see cref="PoolMember"/> objects belonging to a specific <see cref="Pool"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="PoolMember"/> objects</returns>
        Task<IEnumerable<PoolMember>> GetPoolMembersAsync(int poolId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a list of <see cref="PoolMember"/> objects for a specific <see cref="SiteUser"/>
        /// </summary>
        /// <param name="userId"><see cref="SiteUser"/> unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="PoolMember"/> objects</returns>
        Task<IEnumerable<PoolMember>> GetUserMembershipsAsync(string userId, CancellationToken cancellationToken = default);
    }
}
