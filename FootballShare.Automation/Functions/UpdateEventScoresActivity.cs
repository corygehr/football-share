using FootballShare.Automation.Parsers;
using FootballShare.DAL.Services;
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
    /// Refreshes the score for each <see cref="WeekEvent"/>
    /// </summary>
    public class UpdateEventScoresActivity
    {
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance
        /// </summary>
        private readonly ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="WeekEvent"/> scores parser
        /// </summary>
        private readonly ScoresParser _scoresParser;

        /// <summary>
        /// Creates a new <see cref="GetWeekEventsActivity"/> instance
        /// </summary>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> instance</param>
        /// <param name="scoresParser"><see cref="WeekEvent"/> scores parser</param>
        public UpdateEventScoresActivity(ISportsLeagueService leagueService, ScoresParser scoresParser)
        {
            this._leagueService = leagueService;
            this._scoresParser = scoresParser;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <remarks>
        /// Every Tuesday at 5AM UTC (1AM ET)
        /// 0 0 5 * * Tue
        /// </remarks>
        [FunctionName("UpdateEventScoresActivity")]
        public async Task Run([TimerTrigger("* */2 * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"{nameof(UpdateEventScoresActivity)} invoked at {DateTime.UtcNow}");

            // Get previous week
            SeasonWeek previousWeek = await this._leagueService.GetLeaguePreviousWeekAsync("national-football-league", cancellationToken);

            // Get spreads
            IEnumerable<WeekEvent> weekSpreads = await this._scoresParser.GetScoresForWeekAsync(previousWeek, cancellationToken);

            // Update or create spreads
            foreach(WeekEvent game in weekSpreads)
            {
                //await this._leagueService.UpdateEventScoreAsync(game.Id, game.AwayScore, game.HomeScore, cancellationToken);
            }
        }
    }
}
