using FootballShare.Automation.Parsers;
using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Automation.Functions.Activities
{
    /// <summary>
    /// Refreshes the <see cref="Spread"/> for each <see cref="WeekEvent"/>
    /// </summary>
    public class UpdateSpreadsActivity
    {
        /// <summary>
        /// <see cref="SportsLeague"/> service
        /// </summary>
        private readonly ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="Spread"/> parser
        /// </summary>
        private readonly SpreadsParser _spreadsParser;

        /// <summary>
        /// Creates a new <see cref="GetWeekEventsActivity"/> instance
        /// </summary>
        /// <param name="leagueService"><see cref="SportsLeague"/> service</param>
        /// <param name="spreadsParser"><see cref="Spread"/> parser</param>
        public UpdateSpreadsActivity(ISportsLeagueService leagueService, SpreadsParser spreadsParser)
        {
            this._leagueService = leagueService;
            this._spreadsParser = spreadsParser;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <remarks>
        /// Every hour
        /// 0 0 * * * *
        /// </remarks>
        [FunctionName("UpdateSpreadsActivity")]
        public async Task Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"UpdateSpreads invoked at {DateTime.UtcNow}");

            // Get active SeasonWeek
            SeasonWeek currentWeek = await this._leagueService.GetLeagueCurrentSeasonWeekAsync("national-football-league", cancellationToken);

            // Get spreads
            IEnumerable<Spread> weekSpreads = await this._spreadsParser.GetSpreadsForWeekAsync(currentWeek, cancellationToken);

            // Update or create spreads
            foreach(Spread spread in weekSpreads)
            {
                await this._leagueService.UpsertWeekEventSpreadAsync(spread, cancellationToken);
            }
        }
    }
}
