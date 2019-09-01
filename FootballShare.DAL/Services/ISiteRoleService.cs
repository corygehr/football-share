using FootballShare.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="SiteRole"/> service interface
    /// </summary>
    public interface ISiteRoleService : IService, IRoleStore<SiteRole>
    {
    }
}
