using FootballShare.DAL.Exceptions;
using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
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

        // GET: Betting/Cancel/5
        [HttpGet("Betting/Cancel/{id:int}")]
        public async Task<ActionResult> Cancel(int id)
        {
            // Confirm current user owns the bet
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Wager wager = await this._bettingService.GetWagerAsync(id.ToString());

            if (wager.SiteUserId == user.Id)
            {
                // Get event and return data
                WeekEvent weekEvent = await this._bettingService.GetWeekEventAsync(wager.WeekEventId);

                BettingCancelViewModel vm = new BettingCancelViewModel
                {
                    Event = weekEvent,
                    Wager = wager
                };

                return View(vm);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: Betting/Cancel/5
        [HttpPost("Betting/Cancel/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int id, IFormCollection collection)
        {
            // Confirm current user owns the bet
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Wager wager = await this._bettingService.GetWagerAsync(id.ToString());

            if (wager.SiteUserId == user.Id)
            {
                try
                {
                    await this._bettingService.RemoveWagerAsync(wager.Id);

                    TempData.Put("UserMessage", new UserMessageViewModel
                    {
                        CssClassName = "alert-success",
                        Title = "Success!",
                        Message = $"Bet cancelled successfully."
                    });

                    return RedirectToAction(nameof(Events), new { seasonWeekId = wager.Event.SeasonWeekId, poolId = wager.PoolId });
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
                return NotFound();
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
                IEnumerable<Spread> events = await this._bettingService.GetWeekSpreadsAsync(seasonWeekId);
                IEnumerable<Wager> weekWagers = await this._bettingService.GetUserWagersForWeekByPoolAsync(user.Id, seasonWeekId, poolId);

                // Get SeasonWeek details
                SeasonWeekEventsViewModel vm = new SeasonWeekEventsViewModel
                {
                    EventSpreads = events.ToList(),
                    PoolMembership = userMembership,
                    WeekWagers = weekWagers.ToList()
                };

                return View(vm);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Betting/History/nfl-2019-1/3
        [HttpGet("Betting/History/{seasonWeekId}/{poolId:int}")]
        public async Task<ActionResult> History(string seasonWeekId, int poolId)
        {
            // Confirm user belongs to pool
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            PoolMember userMembership = await this._poolService.GetPoolMemberAsync(poolId, user.Id);

            if (userMembership != null)
            {
                // Get all wagers from the previous week
                IEnumerable<Wager> wagers = await this._bettingService.GetPoolWagersForWeekAsync(poolId, seasonWeekId);
                BettingHistoryViewModel vm = new BettingHistoryViewModel
                {
                    Pool = userMembership.Pool,
                    Wagers = wagers.ToList(),
                    Week = await this._bettingService.GetSeasonWeekAsync(seasonWeekId)
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

            if (userMembership == null)
            {
                return NotFound();
            }

            // Get event
            WeekEvent weekEvent = await this._bettingService.GetWeekEventAsync(eventId);

            if (weekEvent.Time <= DateTimeOffset.Now)
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
                        seasonWeekId = weekEvent.SeasonWeekId,
                        poolId = poolId
                    }
                );
            }

            // Get event spread
            Spread eventSpread = await this._bettingService.GetSpreadForEventAsync(eventId);

            if (!eventSpread.AwaySpread.HasValue || !eventSpread.HomeSpread.HasValue)
            {
                // Spreads have not yet been pulled for this event
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "No Spreads",
                    Message = $"Spreads are not yet available for this event. Please check back soon."
                });

                return RedirectToAction(
                    nameof(Events),
                    new
                    {
                        seasonWeekId = weekEvent.SeasonWeekId,
                        poolId = poolId
                    });
            }

            // Check if user has already submitted their bet limit for the week
            IEnumerable<Wager> wagersForWeek = await this._bettingService
                .GetUserWagersForWeekByPoolAsync(user.Id, weekEvent.SeasonWeekId, poolId);

            if(wagersForWeek.Count() >= userMembership.Pool.WagersPerWeek)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "Too Many Bets",
                    Message = $"You have already submitted all of your bets for this week. Cancel one to place another."
                });

                return RedirectToAction(
                    nameof(Events),
                    new
                    {
                        seasonWeekId = weekEvent.SeasonWeekId,
                        poolId = poolId
                    }
                );
            }

            // Check if user has already submitted a bet for this Event
            Wager existingWager = wagersForWeek.Where(w => w.WeekEventId == eventId).FirstOrDefault();
            if (existingWager != null)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "Bet Exists",
                    Message = $"You have already submitted a ${existingWager.Amount} bet for {existingWager.Event.ToString()}. Please cancel first before changing it."
                });

                return RedirectToAction(
                    nameof(Events),
                    new
                    {
                        seasonWeekId = weekEvent.SeasonWeekId,
                        poolId = poolId
                    }
                );
            }

            // Checks pass, allow page to load
            PlaceBetViewModel vm = new PlaceBetViewModel
            {
                Event = weekEvent,
                PoolId = poolId,
                PoolMembership = userMembership,
                Spread = eventSpread,
                WeekEventId = eventId
            };
            return View(vm);
        }

        // POST: Betting/Place
        [HttpPost("Betting/Place/{eventId:int}/{poolId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Place(PlaceBetViewModel submission)
        {
            // Check if user is part of pool
            SiteUser user = await this._userManager.GetUserAsync(HttpContext.User);
            PoolMember userMembership = await this._poolService.GetUserPoolProfileAsync(user.Id, submission.PoolId);

            if (userMembership == null)
            {
                return NotFound();
            }

            Spread eventSpread = await this._bettingService.GetSpreadForEventAsync(submission.WeekEventId);

            if (eventSpread == null)
            {
                return NotFound();
            }
            else if(!eventSpread.AwaySpread.HasValue || !eventSpread.HomeSpread.HasValue)
            {
                // Spreads have not yet been pulled for this event
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "No Spreads",
                    Message = $"Spreads are not yet available for this event. Please check back soon."
                });

                return RedirectToAction(
                    nameof(Events),
                    new
                    {
                        seasonWeekId = eventSpread.Event.SeasonWeekId,
                        poolId = submission.PoolId
                    });
            }

            // Check if bet can no longer be submitted
            if (eventSpread.Event.Time <= DateTimeOffset.Now)
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
                        poolId = submission.PoolId
                    }
                );
            }

            // Check if user has already submitted their bet limit for the week
            IEnumerable<Wager> wagersForWeek = await this._bettingService
                .GetUserWagersForWeekByPoolAsync(
                    user.Id,
                    eventSpread.Event.SeasonWeekId,
                    submission.PoolId
                );

            if (wagersForWeek.Count() >= userMembership.Pool.WagersPerWeek)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "Too Many Bets",
                    Message = $"You have already submitted all of your bets for this week. Cancel one to place another."
                });

                return RedirectToAction(
                    nameof(Events),
                    new
                    {
                        seasonWeekId = eventSpread.Event.SeasonWeekId,
                        poolId = submission.PoolId
                    }
                );
            }

            // Check if user has already submitted a bet for this Event
            Wager existingWager = wagersForWeek
                .Where(w => w.WeekEventId == submission.WeekEventId)
                .FirstOrDefault();

            if (existingWager != null)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "Bet Exists",
                    Message = $"You have already submitted a ${existingWager.Amount} bet for {existingWager.Event.ToString()}. Please cancel first before changing it."
                });

                return RedirectToAction(
                    nameof(Events),
                    new
                    {
                        seasonWeekId = eventSpread.Event.SeasonWeekId,
                        poolId = submission.PoolId
                    }
                );
            }

            // Get target spread
            double targetSpread = 0.0;

            if(eventSpread.Event.AwayTeamId == submission.SelectedTeamId)
            {
                targetSpread = eventSpread.AwaySpread.Value;
            }
            else
            {
                targetSpread = eventSpread.HomeSpread.Value;
            }

            Wager wager = new Wager
            {
                Amount = submission.WagerAmount,
                PoolId = submission.PoolId,
                Result = WagerResult.None,
                SelectedTeamId = submission.SelectedTeamId,
                SelectedTeamSpread = targetSpread,
                SiteUserId = user.Id,
                WeekEventId = submission.WeekEventId
            };

            try
            {
                await this._bettingService.PlaceWagerAsync(wager);

                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-success",
                    Title = "Success!",
                    Message = $"Successfully placed bet."
                });

                return RedirectToAction(nameof(Events), new { seasonWeekId = eventSpread.Event.SeasonWeekId, poolId = submission.PoolId });
            }
            catch (NotEnoughFundsException ex)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "Warning",
                    Message = ex.Message
                });

                return RedirectToAction(nameof(Place), new { eventId = submission.WeekEventId, poolId = submission.PoolId });
            }
            catch (MaxBetsPlacedForWeekException ex)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-warning",
                    Title = "Warning",
                    Message = ex.Message
                });

                return RedirectToAction(nameof(Place), new { eventId = submission.WeekEventId, poolId = submission.PoolId });
            }
            catch (Exception)
            {
                TempData.Put("UserMessage", new UserMessageViewModel
                {
                    CssClassName = "alert-danger",
                    Title = "Error",
                    Message = $"Failed to place wager. Please try again."
                });

                return RedirectToAction(nameof(Place), new { eventId = submission.WeekEventId, poolId = submission.PoolId });
            }
        }
    }
}