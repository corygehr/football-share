using FootballShare.DAL.Exceptions;
using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;
using FootballShare.Web.Extensions;
using FootballShare.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballShare.Web.Controllers
{
    public class BettingController : Controller
    {
        /// <summary>
        /// Betting service object
        /// </summary>
        private IBettingService _bettingService;
        /// <summary>
        /// Pool service object
        /// </summary>
        private IPoolService _poolService;
        /// <summary>
        /// User Manager object
        /// </summary>
        private UserManager<SiteUser> _userManager;

        /// <summary>
        /// Creates a new <see cref="BettingController"/> instance
        /// </summary>
        /// <param name="bettingService">Betting service instance</param>
        /// <param name="poolService">Pool service instance</param>
        /// <param name="userManager">User Manager instance</param>
        public BettingController(IBettingService bettingService, IPoolService poolService, UserManager<SiteUser> userManager)
        {
            this._bettingService = bettingService;
            this._poolService = poolService;
            this._userManager = userManager;
        }

        // GET: Betting/CancelBet/5
        [HttpGet("Betting/Cancel/{id:int}")]
        public async Task<ActionResult> CancelBet(int id)
        {
            // Confirm current user owns the bet
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Wager wager = await this._bettingService.GetWagerAsync(id.ToString());

            if (wager.SiteUserId == user.Id)
            {
                return View(wager);
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: Betting/CancelBet/5
        [HttpPost("Betting/Cancel/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelBet(int id, IFormCollection collection)
        {
            // Confirm current user owns the bet
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Wager wager = await this._bettingService.GetWagerAsync(id.ToString());

            if (wager.SiteUserId == user.Id)
            {
                try
                {
                    await this._bettingService.RemoveWagerAsync(wager);

                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-success",
                        Title = "Success!",
                        Message = $"Bet cancelled successfully."
                    });

                    return RedirectToAction(nameof(Events), new { id = wager.Event.SeasonWeekId });
                }
                catch
                {
                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-danger",
                        Title = "Error",
                        Message = $"Failed to cancel event. Please try again later."
                    });

                    return View(wager);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: Betting/Events/nfl-2019-1/3
        [HttpGet("Betting/Events/{seasonWeekId}/{poolId:int}")]
        public async Task<ActionResult> Events(string seasonWeekId, int poolId)
        {
            // Confirm user belongs to pool
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            PoolMember userMembership = await this._poolService.GetPoolMemberAsync(poolId, user.Id);

            if (userMembership != null)
            {
                IEnumerable<Spread> events = await this._bettingService.GetWeekSpreads(seasonWeekId);

                // Get SeasonWeek details
                SeasonWeekEventsViewModel vm = new SeasonWeekEventsViewModel
                {
                    EventSpreads = events.ToList(),
                    PoolMembership = userMembership
                };

                return View(vm);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Betting/Place/2/1
        [HttpGet("Betting/Place/{eventId:int}/{poolId:int}")]
        public async Task<ActionResult> Place(int eventId, int poolId)
        {
            // Check if user is part of pool
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            PoolMember userMembership = await this._poolService.GetUserPoolProfileAsync(user.Id, poolId);

            if(userMembership != null)
            {
                // Check if betting is open for the event
                Spread eventSpread = await this._bettingService.GetSpreadForEventAsync(eventId);
                if (eventSpread.Event.Time > DateTimeOffset.Now)
                {
                    // Return ViewModel
                    PlaceBetViewModel vm = new PlaceBetViewModel
                    {
                        Spread = eventSpread,
                        WeekEventId = eventId
                    };
                    return View(vm);
                }
                else
                {
                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-warning",
                        Title = "Deadline passed",
                        Message = $"Betting deadline for this event has already passed."
                    });

                    return RedirectToAction(
                        nameof(Events),
                        new
                        {
                            seasonWeekId = eventSpread.Event.SeasonWeekId,
                            poolId = poolId
                        }
                    );
                }
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Betting/Place/2/1
        [HttpPost("Betting/Place/{eventId:int}/{poolId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Place(int eventId, int poolId, PlaceBetViewModel submission)
        {
            Spread eventSpread = await this._bettingService.GetSpreadForEventAsync(submission.WeekEventId);

            if(eventSpread != null)
            {
                SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);

                Wager wager = new Wager
                {
                    Amount = submission.WagerAmount,
                    AwaySpread = eventSpread.AwaySpread,
                    HomeSpread = eventSpread.HomeSpread,
                    SiteUserId = user.Id,
                    WeekEventId = submission.WeekEventId
                };

                if (eventSpread.Event.Time < DateTimeOffset.Now)
                {
                    try
                    {
                        await this._bettingService.PlaceWagerAsync(wager);

                        TempData.Put("UserMessage", new UserMessageViewModel
                        {
                            CssClassName = "alert-success",
                            Title = "Success!",
                            Message = $"Successfully placed bet!"
                        });

                        return RedirectToAction(nameof(Events), new { id = eventSpread.Event.SeasonWeekId });
                    }
                    catch (NotEnoughFundsException ex)
                    {
                        TempData.Put("UserMessage", new UserMessageViewModel
                        {
                            CssClassName = "alert-warning",
                            Title = "Warning",
                            Message = ex.Message
                        });

                        return RedirectToAction(nameof(Place), new { id = eventSpread.WeekEventId });
                    }
                    catch (MaxBetsPlacedForWeekException ex)
                    {
                        TempData.Put("UserMessage", new UserMessageViewModel
                        {
                            CssClassName = "alert-warning",
                            Title = "Warning",
                            Message = ex.Message
                        });

                        return RedirectToAction(nameof(Place), new { id = eventSpread.WeekEventId });
                    }
                    catch (Exception)
                    {
                        TempData.Put("UserMessage", new UserMessageViewModel
                        {
                            CssClassName = "alert-danger",
                            Title = "Error",
                            Message = $"Failed to place wager. Please try again."
                        });

                        return RedirectToAction(nameof(Place), new { id = eventSpread.WeekEventId });
                    }
                }
                else
                {
                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-warning",
                        Title = "Deadline passed",
                        Message = $"Betting deadline for this event has already passed."
                    });

                    return RedirectToAction(nameof(Events), new { id = eventSpread.Event.SeasonWeekId });
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}