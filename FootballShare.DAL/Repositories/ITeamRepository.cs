using FootballShare.Entities.Leagues;

using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Base <see cref="Team"/> repository
    /// </summary>
    public interface ITeamRepository : IRepository<Team>
    {
        /// <summary>
        /// Gets a <see cref="Team"/> by name
        /// </summary>
        /// <param name="name"><see cref="Team"/> name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="Team"/> or null</returns>
        Task<Team> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
