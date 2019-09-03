using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.League;
using FootballShare.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public BettingController(IBettingService bettingService)
        {
            this._bettingService = bettingService;
        }

        // GET: Betting
        public ActionResult Index()
        {
            return View();
        }

        // GET: Betting/Events/5
        public async Task<ActionResult> Events(string id)
        {
            IEnumerable<Spread> events = await this._bettingService.GetWeekSpreads(id);

            // Get SeasonWeek details
            SeasonWeekEventsViewModel vm = new SeasonWeekEventsViewModel
            {
                EventSpreads = events.ToList()
            };

            return View(vm);
        }

        // GET: Betting/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Betting/Create
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

        // GET: Betting/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Betting/Edit/5
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

        // GET: Betting/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Betting/Delete/5
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