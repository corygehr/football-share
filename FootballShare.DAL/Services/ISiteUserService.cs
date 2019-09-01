using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace FootballShare.DAL.Services
{
    public interface ISiteUserService : IService, IUserStore<SiteUser>, IUserEmailStore<SiteUser>,
          IUserLoginStore<SiteUser>, IUserRoleStore<SiteUser>
    {
    }
}
