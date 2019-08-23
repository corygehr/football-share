using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="T">Repository type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Creates a new <see cref="T"/>
        /// </summary>
        /// <param name="newEntity">Entity to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created entity</returns>
        T CreateAsync(T newEntity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a <see cref="T"/> by unique identifier
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Single <see cref="T"/> entry</returns>
        Task<T> FindByIdAsync(string id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the provided <see cref="T"/> object
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}
