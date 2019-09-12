using FootballShare.DAL.Repositories;
using FootballShare.Entities.Leagues;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Automation.Functions.Activities
{
    /// <summary>
    /// Downloads all <see cref="WeekEvents"/> for a given <see cref="Season"/>
    /// </summary>
    public class GetWeekEventsActivity
    {
        /// <summary>
        /// <see cref="Season"/> repository
        /// </summary>
        private readonly ISeasonRepository _seasonRepo;
        /// <summary>
        /// <see cref="SeasonWeek"/> repository
        /// </summary>
        private readonly ISeasonWeekRepository _seasonWeekRepo;
        /// <summary>
        /// <see cref="WeekEvent"/> repository
        /// </summary>
        private readonly IWeekEventRepository _weekEventRepo;

        /// <summary>
        /// Creates a new <see cref="GetWeekEventsActivity"/> instance
        /// </summary>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        /// <param name="seasonWeekRepo"><see cref="SeasonWeek"/> repository</param>
        /// <param name="weekEventRepo"><see cref="WeekEvent"/> repository</param>
        public GetWeekEventsActivity(ISeasonRepository seasonRepo, ISeasonWeekRepository seasonWeekRepo, IWeekEventRepository weekEventRepo)
        {
            this._seasonRepo = seasonRepo;
            this._seasonWeekRepo = seasonWeekRepo;
            this._weekEventRepo = weekEventRepo;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        [FunctionName("GetSeasonEventsActivity")]
        public async Task Run([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"GetSeasonEventsActivity executed at: {DateTime.UtcNow}");

            // Get current season
            Season currentSeason = await this._seasonRepo
                .GetCurrentLeagueSeasonAsync("national-football-league", cancellationToken);

            // Get season weeks
            IEnumerable<SeasonWeek> weeks = await this._seasonWeekRepo.GetAllForSeasonAsync(currentSeason.Id, cancellationToken);

            // Pull data for each week
            HtmlWeb htmlWeb = new HtmlWeb();
            string urlTemplate = "http://www.vegasinsider.com/nfl/matchups/matchups.cfm/week/{0}/season/" + currentSeason.StartDate.Year;

            foreach(SeasonWeek week in weeks)
            {
                // Parse HTML document
                string currentUrl = String.Format(urlTemplate, week.Sequence);
                HtmlDocument htmlDoc = await htmlWeb.LoadFromWebAsync(currentUrl);

                HtmlNode scheduleNode = htmlDoc
                    .DocumentNode
                    .SelectSingleNode("//td[@class='main-content-cell']/div[@class='SLTables1' and not(@id)]");

                // Within node, begin pulling events
            }
        }
    }
}
