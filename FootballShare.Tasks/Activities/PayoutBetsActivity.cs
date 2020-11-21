using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Tasks.Helpers;
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
        public async Task Run([ActivityTrigger] IDurableActivityContext context, ILogger log, CancellationToken cancellationToken = default)
        {
            log.LogInformation($"{nameof(PayoutBetsActivity)} executed at: {DateTime.Now}");

            // Retrieve the scores which were recently pulled
            IEnumerable<WeekEvent> finishedEvents = context.GetInput<IEnumerable<WeekEvent>>();
            log.LogDebug("{0} event(s) to process.", finishedEvents.Count());

            // Process payouts for each event whose score was just pulled
            foreach (WeekEvent evt in finishedEvents)
            {
                IEnumerable<Wager> wagers = await this._bettingService.GetWagersForWeekEventAsync(evt.Id, cancellationToken);
                log.LogDebug("{0} wager(s) made on event {1}.", wagers.Count(), evt.Id);

                // Payout wagers which haven't already been resolved for this event
                IEnumerable<Wager> unpaidWagers = wagers.Where(w => w.Result == WagerResult.None);
                log.LogDebug("{0} wager(s) to process for event {1}.", unpaidWagers.Count(), evt.Id);

                foreach (Wager wager in unpaidWagers)
                {
                    // If event was postponed, refund
                    if (evt.Postponed)
                    {
                        wager.Result = WagerResult.Refund;
                    }
                    else
                    {
                        double chosenTeamScore = 0;
                        double opponentScore = 0;

                        if (wager.SelectedTeamId == evt.HomeTeamId)
                        {
                            // User picked home team
                            chosenTeamScore = evt.HomeScore;
                            opponentScore = evt.AwayScore;
                        }
                        else
                        {
                            // User picked away team
                            chosenTeamScore = evt.AwayScore;
                            opponentScore = evt.HomeScore;
                        }

                        // Determine result based on score after considering spread
                        wager.Result = SpreadsHelper.DetermineWagerResult(
                            wager.SelectedTeamSpread,
                            chosenTeamScore,
                            opponentScore
                        );
                    }

                    log.LogDebug("Bet {0} result: {1}.", wager.Id, wager.Result.ToString());

                    // Determine payout amount
                    try
                    {
                        decimal payoutAmount = PayoutHelper.GetPayoutForWagerResult(wager.Result, wager.Amount);

                        // Do nothing if payout amount is zero
                        if (payoutAmount != 0)
                        {
                            this._bettingService.PayoutWagerAsync(wager, payoutAmount, cancellationToken).GetAwaiter().GetResult();
                        }

                        // Update wager object
                        this._bettingService.UpdateWagerAsync(wager).GetAwaiter().GetResult();
                    }
                    catch (InvalidWagerResultPayoutException ex)
                    {
                        log.LogError(ex, "Encountered an error processing payout for WagerId {0}.", wager.Id);
                    }
                }
            }
        }
    }
}