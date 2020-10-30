using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Automation.Functions
{
    /// <summary>
    /// Confirms <see cref="Wager"/> amounts to be paid
    /// </summary>
    public class UpdatePayoutsActivity
    {
        /// <summary>
        /// <see cref="IBettingService"/> instance
        /// </summary>
        private readonly IBettingService _bettingService;
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance
        /// </summary>
        private readonly ISportsLeagueService _leagueService;

        /// <summary>
        /// Creates a new <see cref="UpdatePayoutsActivity"/> instance
        /// </summary>
        /// <param name="bettingService">Betting service</param>
        /// <param name="leagueService">Sport League service</param>
        public UpdatePayoutsActivity(IBettingService bettingService, ISportsLeagueService leagueService)
        {
            this._bettingService = bettingService;
            this._leagueService = leagueService;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="myTimer">Invoking timer</param>
        /// <param name="log">Log provider</param>
        /// <remarks>
        /// Every Tuesday at 12PM UTC (8AM ET)
        /// 0 0 12 * * Tue
        /// </remarks>
        [FunctionName("UpdatePayoutsActivity")]
        public async Task Run([TimerTrigger("0 0 12 * * Tue")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"{nameof(UpdatePayoutsActivity)} executed at: {DateTime.Now}");

            // Get all unresolved wagers
            IEnumerable<Wager> wagers = await this._bettingService.GetUnresolvedWagersAsync(cancellationToken);

            // TODO: Implement transaction logic to avoid needing to run things Synchronously
            foreach(Wager wager in wagers)
            {
                // Get event data
                WeekEvent wagerEvent = await this._leagueService.GetWeekEventAsync(wager.WeekEventId, cancellationToken);

                // Calculate payout if event has concluded
                if (wagerEvent.Time.AddHours(6) < DateTimeOffset.UtcNow)
                {
                    // Was the event played?
                    if(wagerEvent.Postponed)
                    {
                        // Postponed events are refunded in full
                        wager.Result = WagerResult.Refund;

                        // Update ledger and pool member
                        this._bettingService.PayoutWagerAsync(wager, wager.Amount).GetAwaiter().GetResult();
                    }
                    else
                    {
                        double chosenTeamScore = 0;
                        double opponentScore = 0;

                        if (wager.SelectedTeamId == wagerEvent.HomeTeamId)
                        {
                            // User picked home team
                            chosenTeamScore = wagerEvent.HomeScore;
                            opponentScore = wagerEvent.AwayScore;
                        }
                        else
                        {
                            chosenTeamScore = wagerEvent.AwayScore;
                            opponentScore = wagerEvent.HomeScore;
                        }

                        double chosenTeamScoreWithSpread = chosenTeamScore + wager.SelectedTeamSpread;

                        if (chosenTeamScoreWithSpread > opponentScore)
                        {
                            wager.Result = WagerResult.Win;

                            // Update ledger and pool member
                            this._bettingService.PayoutWagerAsync(wager, wager.Amount * 2).GetAwaiter().GetResult();
                        }
                        else if (chosenTeamScoreWithSpread == opponentScore)
                        {
                            wager.Result = WagerResult.Push;

                            // Update ledger and pool member
                            this._bettingService.PayoutWagerAsync(wager, wager.Amount).GetAwaiter().GetResult();
                        }
                        else
                        {
                            wager.Result = WagerResult.Loss;
                        }
                    }

                    // Update wager
                    this._bettingService.UpdateWagerAsync(wager).GetAwaiter().GetResult();
                }
            }
        }
    }
}
