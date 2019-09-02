using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballShare.DAL.Services;
using FootballShare.Entities.Group;
using FootballShare.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FootballShare.Web.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        /// <summary>
        /// <see cref="BettingGroup"/> Management service
        /// </summary>
        private readonly IGroupManagementService _groupService;
        /// <summary>
        /// Identity manager
        /// </summary>
        private readonly UserManager<SiteUser> _userManager;

        /// <summary>
        /// Creates a new <see cref="GroupsController"/> instance
        /// </summary>
        /// <param name="groupService">Group Management service</param>
        public GroupsController(IGroupManagementService groupService, UserManager<SiteUser> userManager)
        {
            this._groupService = groupService;
            this._userManager = userManager;
        }

        // GET: Groups
        public async Task<ActionResult> Index()
        {
            // Get current user ID
            var user = this._userManager.GetUserAsync(HttpContext.User);

            return View(await this._groupService.GetUserGroupsAsync(user.Id.ToString()));
        }

        // GET: Groups/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Get specific group and details
            return View(await this._groupService.GetGroupAsync(id));
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Groups/Edit/5
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

        // GET: Groups/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Groups/Delete/5
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