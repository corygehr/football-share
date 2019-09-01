using FootballShare.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FootballShare.Entities.User;

namespace FootballShare.Web.Controllers
{
    [Authorize]
    public class PoolsController : Controller
    {
        /// <summary>
        /// <see cref="BettingGroup"/> data repository
        /// </summary>
        private readonly IBettingGroupRepository _groupRepo;
        /// <summary>
        /// Identity manager
        /// </summary>
        private readonly UserManager<SiteUser> _userManager;

        /// <summary>
        /// Creates a new <see cref="PoolsController"/> instance
        /// </summary>
        /// <param name="groupRepo"><see cref="BettingGroup"/> repository</param>
        /// <param name="userManager">Identity manager</param>
        public PoolsController(IBettingGroupRepository groupRepo, UserManager<SiteUser> userManager)
        {
            this._groupRepo = groupRepo;
            this._userManager = userManager;
        }

        // GET: Pools
        public async Task<ActionResult> Index()
        {
            // Get current user ID
            var user = this._userManager.GetUserAsync(HttpContext.User);

            return View(await this._groupRepo
                .SearchByMemberUserIdAsync(
                    user.Id.ToString()
                )
            );
        }

        // GET: Pools/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if(ModelState.IsValid)
                {

                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pools/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pools/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pools/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}