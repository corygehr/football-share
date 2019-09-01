using FootballShare.Entities.User;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// <see cref="SiteUserLoginProvider"/> repository interface
    /// </summary>
    public interface ISiteUserLoginProviderRepository : IRepository<SiteUserLoginProvider>
    {
        /// <summary>
        /// Retrieves all <see cref="SiteUserLoginProvider"/> objects for the specified <see cref="SiteUser"/>
        /// </summary>
        /// <param name="userId">Target <see cref="SiteUser"/> unique identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SiteUserLoginProvider"/> objects</returns>
        Task<IEnumerable<SiteUserLoginProvider>> GetAllForUserAsync(string userId, CancellationToken cancellationToken = default);
    }
}
