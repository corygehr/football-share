using FootballShare.DAL.Repositories;
using FootballShare.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace FootballShare.Web.Controllers
{
    [Authorize]
    public class PoolsController : Controller
    {
        /// <summary>
        /// Data repository for <see cref="BettingGroup"/> objects
        /// </summary>
        private IBettingGroupRepository _groupRepo;
        /// <summary>
        /// <see cref="UserManager"/> object
        /// </summary>
        private UserManager<SiteUser> _userManager;

        /// <summary>
        /// Creates a new <see cref="PoolsController"/> instance
        /// </summary>
        /// <param name="groupRepo"><see cref="BettingGroup"/> repository</param>
        /// <param name="userManager"><see cref="UserManager"/> instance</param>
        public PoolsController(IBettingGroupRepository groupRepo, UserManager<SiteUser> userManager)
        {
            this._groupRepo = groupRepo;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get all groups for current user
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            return View(await this._groupRepo.SearchByMemberUserIdAsync(user.Id.ToString()));
        }
    }
}