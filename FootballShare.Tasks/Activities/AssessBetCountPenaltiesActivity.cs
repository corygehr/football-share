using FootballShare.DAL.Services;
using FootballShare.Entities.Leagues;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Tasks.Activities
{
    /// <summary>
    /// Logs penalties against players who do not meet the minimum bet count for the <see cref="SeasonWeek"/>
    /// </summary>
    public class AssessBetCountPenaltiesActivity
    {
        /// <summary>
        /// <see cref="IBettingService"/> instance for data context
        /// </summary>
        private IBettingService _bettingService;
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance for data context
        /// </summary>
        private ISportsLeagueService _leagueService;

        /// <summary>
        /// Creates a new <see cref="AssessBetCountPenaltiesActivity"/> instance
        /// </summary>
        /// <param name="bettingService"><see cref="IBettingService"/> instance</param>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> instance</param>
        public AssessBetCountPenaltiesActivity(IBettingService bettingService, ISportsLeagueService leagueService)
        {
            this._bettingService = bettingService;
            this._leagueService = leagueService;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="context">Durable activity context object</param>
        /// <param name="log">Log provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task Run([ActivityTrigger]IDurableActivityContext context, ILogger log, CancellationToken cancellationToken = default)
        {
            log.LogInformation($"{nameof(FetchEventScoresActivity)} executed at: {DateTime.Now}");

            // Get target league(s)
            string[] targetLeagues = context.GetInput<string[]>();

            foreach(string leagueId in targetLeagues)
            {
                log.LogDebug("Determining bet penalties for {0}.", leagueId);
                // Get count of completed events for the current SeasonWeek in the targeted league
                SeasonWeek currentWeek = await this._leagueService.GetLeagueCurrentSeasonWeekAsync(leagueId, cancellationToken);
                log.LogDebug("{0} is currently in SeasonWeek {1}.", leagueId, currentWeek.Id);
                IEnumerable<WeekEvent> events = await this._leagueService.GetWeekEventsAsync(currentWeek.Id, cancellationToken);
                int completedEventCount = events.Where(e => e.Completed).Count();
                log.LogDebug("{0} has completed {1} event(s) in {2}.", leagueId, completedEventCount, currentWeek.Id);
                
                // Begin assessing penalties if all events have been completed and no events have been postponed
                if(completedEventCount == )
            }
        }
    }
}