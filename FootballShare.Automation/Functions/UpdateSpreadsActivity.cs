using FootballShare.Automation.Parsers;
using FootballShare.DAL.Repositories;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
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
        /// <see cref="Season"/> repository
        /// </summary>
        private readonly ISeasonRepository _seasonRepo;
        /// <summary>
        /// <see cref="SeasonWeek"/> repository
        /// </summary>
        private readonly ISeasonWeekRepository _seasonWeekRepo;
        /// <summary>
        /// <see cref="Spread"/> repository
        /// </summary>
        private readonly ISpreadRepository _spreadRepo;
        /// <summary>
        /// <see cref="Spread"/> parser
        /// </summary>
        private readonly SpreadsParser _spreadsParser;

        /// <summary>
        /// Creates a new <see cref="GetWeekEventsActivity"/> instance
        /// </summary>
        /// <param name="seasonRepo"><see cref="Season"/> repository</param>
        /// <param name="seasonWeekRepo"><see cref="SeasonWeek"/> repository</param>
        /// <param name="spreadRepo"><see cref="Spread"/> repository</param>
        /// <param name="spreadsParser"><see cref="Spread"/> parser</param>
        public UpdateSpreadsActivity(IConfiguration configuration, ISeasonRepository seasonRepo, ISeasonWeekRepository seasonWeekRepo, ISpreadRepository spreadRepo, SpreadsParser spreadsParser)
        {
            this._seasonRepo = seasonRepo;
            this._seasonWeekRepo = seasonWeekRepo;
            this._spreadRepo = spreadRepo;
            this._spreadsParser = spreadsParser;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <remarks>
        /// Every hour (0 0 * * * *)
        /// </remarks>
        [FunctionName("UpdateSpreadsActivity")]
        public async Task Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"UpdateSpreads invoked at {DateTime.UtcNow}");

            // Get current season
            Season currentSeason = await this._seasonRepo
                .GetCurrentLeagueSeasonAsync("national-football-league", cancellationToken);

            // Get active week
            SeasonWeek currentWeek = await this._seasonWeekRepo
                .GetCurrentSeasonWeekAsync(currentSeason.Id, cancellationToken);

            // Get spreads
            IEnumerable<Spread> weekSpreads = await this._spreadsParser.GetSpreadsForWeekAsync(currentWeek, cancellationToken);

            // Update or create spreads
            foreach(Spread spread in weekSpreads)
            {
                await this._spreadRepo.UpsertAsync(spread, cancellationToken);
            }
        }
    }
}
