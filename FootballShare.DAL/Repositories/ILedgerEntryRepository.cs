using FootballShare.Entities.Pools;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base repository interface for <see cref="LedgerEntry"/>
    /// </summary>
    public interface ILedgerEntryRepository : IRepository<LedgerEntry>
    {
        /// <summary>
        /// Gets all <see cref="LedgerEntry"/> objects for a specific <see cref="Pool"/>
        /// </summary>
        /// <param name="poolId"><see cref="Pool"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="LedgerEntry"/> objects</returns>
        Task<IEnumerable<LedgerEntry>> GetEntriesForPoolAsync(int poolId, CancellationToken cancellationToken);
    }
}
