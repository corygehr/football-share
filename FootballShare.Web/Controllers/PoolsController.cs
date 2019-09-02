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

            IEnumerable<Pool> publicPools = await this._poolService.GetPublicPoolsNotJoinedAsync(user.Id);
            IEnumerable<PoolMember> userPools = await this._poolService.GetUserMembershipsAsync(user.Id);

            // Get user pools and unjoined pools
            ListPoolsViewModel vm = new ListPoolsViewModel
            {
                PublicPools = publicPools.ToList(),
                UserPools = userPools.ToList()
            };

            return View(vm);
        }

        // GET: Pools/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // Get user
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

            // Confirm user is in Pool
            PoolMember userProfile = await this._poolService.GetUserPoolProfileAsync(user.Id, id);

            if(userProfile != null)
            {
                // Get pool data
                IEnumerable<PoolMember> members = await this._poolService.GetMembersAsync(userProfile.PoolId);

                PoolDetailsViewModel vm = new PoolDetailsViewModel
                {
                    CurrentUserMembership = userProfile,
                    Members = members.ToList(),
                    Pool = userProfile.Pool
                };

                return View(vm);
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: Pools/Create
        public async Task<ActionResult> Create()
        {
            // Get current seasons
            IEnumerable<Season> seasons = await this._leagueService.GetAllCurrentSeasonsAsync();

            CreatePoolViewModel vm = new CreatePoolViewModel
            {
                AvailableSeasons = seasons
                .Select(s => new SelectListItem
                {
                    Value = s.Id,
                    Text = s.Name
                })
                .ToList()
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
                        CssClassName = "alert-success",
                        Title = "Pool Created",
                        Message = "Your pool has been created successfully!"
                    });

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-error",
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
                    AvailableSeasons = seasons
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id,
                        Text = s.Name
                    })
                    .ToList()
                };

                return View(vm);
            }
        }

        // GET: Pools/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            // Get user membership
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            PoolMember member = await this._poolService.GetUserPoolProfileAsync(user.Id, id);

            if(member != null && member.IsAdmin)
            {
                return View(member.Pool);
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: Pools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Pool pool)
        {
            // Get user membership
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            PoolMember member = await this._poolService.GetUserPoolProfileAsync(user.Id, id);

            if (member != null && member.IsAdmin)
            {
                if(ModelState.IsValid)
                {
                    try
                    {
                        Pool updatedPool = await this._poolService.UpdatePoolAsync(pool);

                        TempData.Put("UserMessage", new UserMessageViewModel
                        {
                            CssClassName = "alert-success",
                            Title = "Success!",
                            Message = $"Pool updated succeeded."
                        });

                        return View(updatedPool);
                    }
                    catch
                    {
                        TempData.Put("UserMessage", new UserMessageViewModel
                        {
                            CssClassName = "alert-error",
                            Title = "Error",
                            Message = $"Failed to update the pool. Please try again."
                        });

                        return View(member.Pool);
                    }
                }
                else
                {
                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-warning",
                        Title = "Warning",
                        Message = $"One or more fields failed to validate. Please check them and try again."
                    });

                    return View(pool);
                }
            }
            else
            {
                return Unauthorized();
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

        // GET: Pools/Join/5
        public async Task<ActionResult> Join(int id)
        {
            try
            {
                Pool targetPool = await this._poolService.GetPoolAsync(id);
                SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

                await this._poolService.AddPoolMemberAsync(user, targetPool);

                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-success",
                    Title = "Success!",
                    Message = $"You have joined {targetPool.Name}."
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-error",
                    Title = "Error",
                    Message = "Failed to join the requested pool. Pleae try again later."
                });

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Pools/Leave/5
        public async Task<ActionResult> Leave(int id)
        {
            // Get user
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

            return View(await this._poolService.GetUserPoolProfileAsync(user.Id, id));
        }

        // POST: Pools/Leave/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Leave(int id, IFormCollection collection)
        {
            // Get user
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

            return View(await this._poolService.GetUserPoolProfileAsync(user.Id, id));
        }
    }
}