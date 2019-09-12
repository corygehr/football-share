using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;

namespace FootballShare.Automation.Functions.Activities
{
    /// <summary>
    /// Refreshes the <see cref="Spread"/> for each <see cref="WeekEvent"/>
    /// </summary>
    public static class UpdateSpreadsActivity
    {
        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        [FunctionName("UpdateSpreadsActivity")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"UpdateSpreads invoked at {DateTime.UtcNow}");
        }
    }
}
