using FootballShare.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace FootballShare.DAL.Services
{
    public interface ISiteUserService : IService, IUserStore<SiteUser>, IUserEmailStore<SiteUser>,
          IUserLoginStore<SiteUser>, IUserRoleStore<SiteUser>
    {
    }
}
