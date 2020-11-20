using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Tasks.Parsers;
using Microsoft.Azure.WebJobs;
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
    /// Updates the spread for a collection of <see cref="WeekEvent"/> instances
    /// </summary>
    public class FetchEventSpreadsActivity
    {
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance
        /// </summary>
        private ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="SpreadsParser"/> instance
        /// </summary>
        private SpreadsParser _spreadsParser;

        /// <summary>
        /// Creates a new <see cref="FetchEventSpreadsActivity"/> instance
        /// </summary>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> used for persisting score data</param>
        /// <param name="spreadsParser"><see cref="SpreadsParser"/> used to get score data</param>
        public FetchEventSpreadsActivity(ISportsLeagueService leagueService, SpreadsParser spreadsParser)
        {
            this._leagueService = leagueService;
            this._spreadsParser = spreadsParser;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="context">Durable functions context object</param>
        /// <param name="log">Logging provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of updated <see cref="Spread"/> entities</returns>
        [FunctionName("FetchEventSpreadsActivity")]
        public async Task<IEnumerable<Spread>> Run([ActivityTrigger]IDurableActivityContext context, ILogger log, CancellationToken cancellationToken = default)
        {
            log.LogInformation($"{nameof(FetchEventSpreadsActivity)} executed at: {DateTime.Now}");

            // Get the leagues covered by this invocation
            IEnumerable<string> targetLeagueIds = context.GetInput<IEnumerable<string>>();

            // Store WeekEvents updated by this invocation
            List<Spread> updatedEntities = new List<Spread>();

            foreach(string leagueId in targetLeagueIds)
            {
                log.LogDebug("Fetching spreads for {0}.", leagueId);

                // Get league data
                SeasonWeek currentWeek = await this._leagueService.GetLeagueCurrentSeasonWeekAsync(leagueId, cancellationToken);
                log.LogDebug("Current {0} week is {1}.", leagueId, currentWeek.Sequence);

                // Get spread data
                IEnumerable<Spread> weekSpreads = await this._spreadsParser.GetSpreadsForWeekAsync(currentWeek, cancellationToken);
                log.LogInformation("Received spreads for {0} {1} event(s).", weekSpreads.Count(), leagueId);

                // Merge with updated entities
                updatedEntities.AddRange(weekSpreads);
            }

            log.LogInformation("Received {0} spreads total across {1} league(s).", updatedEntities.Count, targetLeagueIds.Count());

            // Commit updates to database
            foreach(Spread spread in updatedEntities)
            {
                await this._leagueService.UpsertWeekEventSpreadAsync(spread, cancellationToken);
            }

            return updatedEntities;
        }
    }
}