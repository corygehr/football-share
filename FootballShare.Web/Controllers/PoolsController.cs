using FootballShare.DAL.Services;
using FootballShare.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;


namespace FootballShare.Web.Controllers
{
    [Authorize]
    public class PoolsController : Controller
    {
        /// <summary>
        /// <see cref="Pool"/> service object
        /// </summary>
        private readonly IPoolService _poolService;
        /// <summary>
        /// Identity manager
        /// </summary>
        private readonly UserManager<SiteUser> _userManager;

        /// <summary>
        /// Creates a new <see cref="PoolsController"/> instance
        /// </summary>
        /// <param name="poolService"><see cref="Pool"/> service object</param>
        /// <param name="userManager">Identity manager</param>
        public PoolsController(IPoolService poolService, UserManager<SiteUser> userManager)
        {
            this._poolService = poolService;
            this._userManager = userManager;
        }

        // GET: Pools
        public async Task<ActionResult> Index()
        {
            // Get current user ID
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

            return View(await this._poolService.GetUserMembershipsAsync(user.Id));
        }

        // GET: Pools/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await this._poolService.GetPoolAsync(id));
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
        public async Task<ActionResult> Edit(int id)
        {
            return View(await this._poolService.GetPoolAsync(id));
        }

        // POST: Pools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(await this._poolService.GetPoolAsync(id));
            }
        }

        // GET: Pools/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await this._poolService.GetPoolAsync(id));
        }

        // POST: Pools/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                // Delete pool
                await this._poolService.DeletePoolAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}