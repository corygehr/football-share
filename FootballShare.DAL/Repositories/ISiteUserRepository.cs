using FootballShare.Entities.User;

using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// <see cref="SiteUser"/> repository interface
    /// </summary>
    public interface ISiteUserRepository : IRepository<SiteUser>
    {
        /// <summary>
        /// Retrieves a <see cref="SiteUser"/> by their email address
        /// </summary>
        /// <param name="normalizedEmail"><see cref="SiteUser"/> email address</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="SiteUser"/> object</returns>
        Task<SiteUser> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves a <see cref="SiteUser"/> by a <see cref="SiteUserLoginProvider"/> reference
        /// </summary>
        /// <param name="loginProvider"><see cref="SiteUserLoginProvider"/> ID</param>
        /// <param name="providerKey">User <see cref="SiteUserLoginProvider"/> key</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<SiteUser> GetByLoginProviderAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a specific <see cref="SiteUser"/> object by its provider key
        /// </summary>
        /// <param name="loginProvider">Login Provider name</param>
        /// <param name="providerKey">Provider key</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="SiteUser"/></returns>
        Task<SiteUser> GetByProviderKeyAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a <see cref="SiteUser" by their username/>
        /// </summary>
        /// <param name="normalizedUserName"><see cref="SiteUser"/> username</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="SiteUser"/> object</returns>
        Task<SiteUser> GetByUserNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    }
}