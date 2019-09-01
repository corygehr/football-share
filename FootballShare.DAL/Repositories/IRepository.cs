using System.Collections.Generic;
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
        /// Creates the specified <see cref="T"/>
        /// </summary>
        /// <param name="entity"><see cref="T"/> to store</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created <see cref="T"/> object</returns>
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the specified <see cref="T"/>
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Deletes the specified <see cref="T"/>
        /// </summary>
        /// <param name="entityId">Unique identifier of entity to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteAsync(string entityId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a <see cref="T"/> by unique identifier
        /// </summary>
        /// <param name="entityId">Unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Single <see cref="T"/> entry</returns>
        Task<T> GetAsync(string entityId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a <see cref="T"/>
        /// </summary>
        /// <param name="entity"><see cref="T"/> to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Single <see cref="T"/> entry</returns>
        Task<T> GetAsync(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all <see cref="T"/> entities
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="T"/> objects</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the specified <see cref="T"/>
        /// </summary>
        /// <param name="entity"><see cref="T"/> to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated <see cref="T"/> object</returns>
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}
