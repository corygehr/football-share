using FootballShare.DAL.Services;
using FootballShare.Entities.League;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;
using FootballShare.Web.Extensions;
using FootballShare.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FootballShare.Web.Controllers
{
    [Authorize]
    public class PoolsController : Controller
    {
        /// <summary>
        /// <see cref="SportsLeague"/> service object
        /// </summary>
        private readonly ISportsLeagueService _leagueService;
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
        /// <param name="leagueService"><see cref="SportsLeague"/> service object</param>
        /// <param name="poolService"><see cref="Pool"/> service object</param>
        /// <param name="userManager">Identity manager</param>
        public PoolsController(ISportsLeagueService leagueService, IPoolService poolService, UserManager<SiteUser> userManager)
        {
            this._leagueService = leagueService;
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
        public async Task<ActionResult> Create()
        {
            // Get current seasons
            IEnumerable<Season> seasons = await this._leagueService.GetAllCurrentSeasonsAsync();

            CreatePoolViewModel vm = new CreatePoolViewModel
            {
                AvailableSeasons = seasons.Select(s => new SelectListItem
                {
                    Value = s.Id,
                    Text = s.Name
                })
            };
            return View(vm);
        }

        // POST: Pools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePoolViewModel toBuild)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    // Parse into pool
                    Pool toInsert = new Pool
                    {
                        IsPublic = true,
                        Name = toBuild.PoolName,
                        SeasonId = toBuild.SeasonId,
                        StartingBalance = toBuild.StartingBalance
                    };

                    // Get current user ID
                    SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

                    // Insert
                    await this._poolService.CreatePoolAsync(toInsert, user.Id);

                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "success",
                        Title = "Pool Created",
                        Message = "Your pool has been created successfully!"
                    });

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "error",
                        Title = "Error",
                        Message = "One or more problems were found with your responses. Please check them and try again."
                    });
                    throw new ArgumentException();
                }
            }
            catch
            {
                // Get current seasons
                IEnumerable<Season> seasons = await this._leagueService.GetAllCurrentSeasonsAsync();

                CreatePoolViewModel vm = new CreatePoolViewModel
                {
                    AvailableSeasons = seasons.Select(s => new SelectListItem
                    {
                        Value = s.Id,
                        Text = s.Name
                    })
                };

                return View(vm);
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