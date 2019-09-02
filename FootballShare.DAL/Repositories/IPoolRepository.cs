using FootballShare.Entities.Pools;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="Pool"/> interface
    /// </summary>
    public interface IPoolRepository : IRepository<Pool>
    {
        /// <summary>
        /// Gets all public <see cref="Pool"/> objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of public <see cref="Pool"/> objects</returns>
        Task<IEnumerable<Pool>> GetAllPublicAsync(CancellationToken cancellationToken = default);
    }
}
