using FootballShare.Entities.Leagues;
using FootballShare.Tasks.Activities;
using FootballShare.Tasks.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Tasks
{
    /// <summary>
    /// Coordinates functionality which must be run at regular intervals
    /// </summary>
    public class RecurringTaskOrchestrator
    {
        /// <summary>
        /// Number of hours to wait between runs
        /// </summary>
        private int _sleepDurationHours;
        /// <summary>
        /// Collection of league IDs which should be refreshed
        /// </summary>
        private string[] _targetLeagues;

        /// <summary>
        /// Creates a new <see cref="RecurringTaskOrchestrator"/> instance
        /// </summary>
        /// <param name="configuration">Function app configuration</param>
        public RecurringTaskOrchestrator(IConfiguration configuration)
        {
            this._sleepDurationHours = configuration.GetValue<int>(ConfigurationConstants.RefreshIntervalHours);
            this._targetLeagues = configuration.GetValue<string>(ConfigurationConstants.TargetLeagues).Split(';');
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="context">Orchestration context object</param>
        /// <param name="log">Logging provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [FunctionName("RecurringTaskOrchestrator")]
        public async Task Run([OrchestrationTrigger]IDurableOrchestrationContext context, ILogger log, CancellationToken cancellationToken = default)
        {
            log.LogInformation($"{nameof(RecurringTaskOrchestrator)} executed at: {DateTime.Now}");

            try
            {
                // Fetch spreads for targeted leagues
                await context.CallActivityAsync(nameof(FetchEventSpreadsActivity), this._targetLeagues);

                // Fetch event scores for finished events
                IEnumerable<WeekEvent> finishedEvents = await context
                    .CallActivityAsync<IEnumerable<WeekEvent>>(nameof(FetchEventScoresActivity), this._targetLeagues);

                // If new scores were pulled, payout bets
                if(finishedEvents.Count() > 0)
                {
                    await context.CallActivityAsync(nameof(PayoutBetsActivity), finishedEvents);
                }
            }
            catch(Exception ex)
            {
                log.LogError(ex, "An error occurred running {0}. See InnerException for details.", nameof(RecurringTaskOrchestrator));
            }
            finally
            {
                // Sleep for specified duration
                DateTime nextRefresh = context.CurrentUtcDateTime.AddHours(this._sleepDurationHours);
                await context.CreateTimer(nextRefresh, cancellationToken);
            }

            context.ContinueAsNew(null);
        }
    }
}