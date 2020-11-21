using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace FootballShare.Tasks
{
    /// <summary>
    /// Function for triggering the <see cref="RecurringTaskOrchestrator"/> orchestration
    /// </summary>
    public static class OrchestrationStart
    {
        /// <summary>
        /// Function schedule in cron syntax
        /// </summary>
        /// <remarks>
        /// In Debug configuration, run every two minutes.
        /// In Release configuration, run every hour.
        /// </remarks>
#if DEBUG
        public const string SCHEDULE = "0 */2 * * * *";
#else
    public const string SCHEDULE = "0 0 */1 * * *";
#endif

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Timer context</param>
        /// <param name="starter"><see cref="IDurableClient"/> instance used for kicking off durable function orchestrations</param>
        /// <param name="log">Log provider</param>
        [FunctionName("OrchestrationStart")]
        public static async Task Run([TimerTrigger(SCHEDULE)]TimerInfo myTimer,[DurableClient] IDurableClient starter, ILogger log)
        {
            log.LogInformation($"{nameof(OrchestrationStart)} executed at: {DateTime.Now}");
            
            string instanceId = await starter.StartNewAsync(nameof(RecurringTaskOrchestrator));

            log.LogInformation($"Started {nameof(RecurringTaskOrchestrator)} with ID {instanceId}.");
        }
    }
}
