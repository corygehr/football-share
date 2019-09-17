using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;

namespace FootballShare.Automation.Functions
{
    /// <summary>
    /// Confirms <see cref="Wager"/> amounts to be paid
    /// </summary>
    public static class PayoutWagers
    {
        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        [FunctionName("PayoutWagers")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"PayoutWagers executed at: {DateTime.Now}");
        }
    }
}
