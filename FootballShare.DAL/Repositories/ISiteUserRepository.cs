using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// <see cref="SiteUser"/> repository interface
    /// </summary>
    public interface ISiteUserRepository
        : IRepository<SiteUser>, IUserStore<SiteUser>, IUserEmailStore<SiteUser>,
          IUserLoginStore<SiteUser>, IUserPasswordStore<SiteUser>, IUserRoleStore<SiteUser>
    {
    }
}