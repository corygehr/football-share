using FootballShare.DAL.Services;
using FootballShare.Entities.Leagues;
using FootballShare.Tasks.Parsers;
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
    /// Updates score of each <see cref="WeekEvent"/>
    /// </summary>
    public class FetchEventScoresActivity
    {
        /// <summary>
        /// <see cref="ISportsLeagueService"/> instance
        /// </summary>
        private ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="ScoreParser"/> instance
        /// </summary>
        private ScoresParser _scoreParser;

        /// <summary>
        /// Creates a new <see cref="FetchEventScoresActivity"/> instance
        /// </summary>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> used for persisting score data</param>
        /// <param name="scoreParser"><see cref="ScoresParser"/> used to get score data</param>
        public FetchEventScoresActivity(ISportsLeagueService leagueService, ScoresParser scoreParser)
        {
            this._leagueService = leagueService;
            this._scoreParser = scoreParser;
        }

        /// <summary>
        /// Function entry point
        /// </summary>
        /// <param name="context">Durable functions context object</param>
        /// <param name="log">Logging provider</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [FunctionName("FetchEventScoresActivity")]
        public async Task<IEnumerable<WeekEvent>> Run([ActivityTrigger]IDurableActivityContext context, ILogger log, CancellationToken cancellationToken = default)
        {
            log.LogInformation($"{nameof(FetchEventScoresActivity)} executed at: {DateTime.Now}");

            // Get the leagues covered by this invocation
            IEnumerable<string> targetLeagueIds = context.GetInput<IEnumerable<string>>();

            // Store events updated by this invocation
            List<WeekEvent> updatedEvents = new List<WeekEvent>();

            foreach(string leagueId in targetLeagueIds)
            {
                log.LogDebug("Fetching scores for {0}.", leagueId);

                // Get current SeasonWeek to scope event
                SeasonWeek currentWeek = await this._leagueService
                    .GetLeagueCurrentSeasonWeekAsync(leagueId, cancellationToken);
                log.LogDebug("Current {0} week is {1}.", leagueId, currentWeek.Sequence);

                // Get available scores
                IEnumerable<WeekEvent> weekScores = await this._scoreParser
                    .GetScoresForWeekAsync(currentWeek, cancellationToken);
                log.LogDebug("Received score(s) for {0} {1} event(s).", weekScores.Count(), leagueId);

                // Merge with updated entities
                updatedEvents.AddRange(weekScores);
            }

            log.LogInformation(
                "Received {0} score(s) total across {1} league(s)",
                updatedEvents.Count,
                targetLeagueIds.Count()
            );

            // Commit updates to database
            foreach(WeekEvent game in updatedEvents)
            {
                log.LogDebug(
                    "Updating score for {0} (AwayScore: {1}; HomeScore: {2}).",
                    game.ToString(),
                    game.AwayScore,
                    game.HomeScore
                );
                await this._leagueService
                    .UpdateEventScoreAsync(game.Id, game.AwayScore, game.HomeScore, game.Completed, cancellationToken);
            }

            return updatedEvents;
        }
    }
}