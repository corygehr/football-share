using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Tasks.Parsers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Tasks.Activities
{
    /// <summary>
    /// Processes payments for wagers
    /// </summary>
    public class PayoutBetsActivity
    {
        /// <summary>
        /// <see cref="IBettingService"/> instance
        /// </summary>
        private IBettingService _bettingService;
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance
        /// </summary>
        private ISportsLeagueService _leagueService;

        /// <summary>
        /// Creates a new <see cref="PayoutBetsActivity"/> instance
        /// </summary>
        /// <param name="bettingService"><see cref="IBettingService"/> used for persisting wager data</param>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> used for persisting score data</param>
        public PayoutBetsActivity(IBettingService bettingService, ISportsLeagueService leagueService)
        {
            this._bettingService = bettingService;
            this._leagueService = leagueService;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="context">Durable functions context object</param>
        /// <param name="log">Logging provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [FunctionName("PayoutBetsActivity")]
        public async Task Run([ActivityTrigger]IDurableActivityContext context, ILogger log, CancellationToken cancellationToken = default)
        {
            log.LogInformation($"{nameof(PayoutBetsActivity)} executed at: {DateTime.Now}");

            // Retrieve the scores which were recently pulled
            IEnumerable<WeekEvent> finishedEvents = context.GetInput<IEnumerable<WeekEvent>>();

            // Process payouts for each event whose score was just pulled
            foreach(WeekEvent evt in finishedEvents)
            {
                IEnumerable<Wager> wagers = await this._bettingService.GetWagersForWeekEventAsync(evt.Id, cancellationToken);

                // Payout wagers which haven't already been resolved

            }
        }
    }
}