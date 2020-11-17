using FootballShare.Automation.Parsers;
using FootballShare.DAL.Services;
using FootballShare.Entities.Leagues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Application configuration
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// <see cref="ISportsLeagueService"/> used for data persistence
        /// </summary>
        private readonly ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="WeekEvent"/> parser
        /// </summary>
        private readonly WeekEventsParser _weekEventsParser;

        /// <summary>
        /// Creates a new <see cref="GetWeekEventsActivity"/> instance
        /// </summary>
        /// <param name="configuration">Application configuration</param>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> implementation</param>
        /// <param name="weekEventsParser"><see cref="WeekEvent"/> parser</param>
        public GetWeekEventsActivity(IConfiguration configuration, ISportsLeagueService leagueService, WeekEventsParser weekEventsParser)
        {
            this._configuration = configuration;
            this._leagueService = leagueService;
            this._weekEventsParser = weekEventsParser;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        /// <remarks>
        /// Annually, on September 1 at midnight UTC
        /// 0 0 0 1 Sep *
        /// </remarks>
        [FunctionName("GetWeekEventsActivity")]
        public async Task Run([TimerTrigger("0 0 0 1 Sep *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"{nameof(GetWeekEventsActivity)} executed at: {DateTime.UtcNow}");

            // Get current season
            Season currentSeason = await this._leagueService
                .GetLeagueCurrentSeasonAsync("national-football-league", cancellationToken);

            // Get season weeks
            IEnumerable<SeasonWeek> weeks = await this._leagueService.GetSeasonWeeksAsync(currentSeason.Id, cancellationToken);

            // Get possible starting week limiter
            int startingWeek = 1;
            if(!String.IsNullOrEmpty(this._configuration["StartingWeek"]))
            {
                startingWeek = Int32.Parse(this._configuration["StartingWeek"]);
            }

            // Get all WeekEvents
            List<WeekEvent> allEvents = new List<WeekEvent>();

            foreach(SeasonWeek week in weeks.Where(w => w.Sequence >= startingWeek && w.IsChampionship == false && w.IsPlayoff == false && w.IsPreseason == false))
            {
                allEvents.AddRange(await this._weekEventsParser.GetEventsAsync(week, cancellationToken));
            }

            // Commit all to database
            foreach(WeekEvent weekEvent in allEvents)
            {
                await this._leagueService.CreateWeekEventAsync(weekEvent, cancellationToken);
            }
        }
    }
}
