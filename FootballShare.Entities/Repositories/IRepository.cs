using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Entities.Repositories
{
    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="T">Repository type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets a <see cref="T"/> by unique identifier
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Single <see cref="T"/> entry</returns>
        Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    }
}
