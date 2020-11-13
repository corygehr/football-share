using FootballShare.Entities.Leagues;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="TeamAlias"/> repository interface
    /// </summary>
    public interface ITeamAliasRepository : IRepository<TeamAlias>
    {
        /// <summary>
        /// Gets a <see cref="TeamAlias"/> by name
        /// </summary>
        /// <param name="teamAlias">Alias value</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="TeamAlias"/> or null</returns>
        Task<IEnumerable<TeamAlias>> GetByAliasAsync(string teamAlias, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="TeamAlias"/> values by <see cref="Team"/> unique identifier
        /// </summary>
        /// <param name="teamId">Unique <see cref="Team"/> identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="TeamAlias"/> objects</returns>
        Task<IEnumerable<TeamAlias>> GetTeamAliasesAsync(string teamId, CancellationToken cancellationToken = default);
    }
}
