using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// <see cref="SiteRole"/> base repository interface
    /// </summary>
    public interface ISiteRoleRepository
        : IRepository<SiteRole>, IRoleStore<SiteRole>
    {
    }
}
