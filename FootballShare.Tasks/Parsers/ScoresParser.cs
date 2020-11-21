using FootballShare.DAL.Services;
using FootballShare.Entities.Leagues;
using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Tasks.Parsers
{
    /// <summary>
    /// Pulls <see cref="WeekEvent"/> score data from Vegas Insider
    /// </summary>
    public class ScoresParser : HtmlParser
    {
        /// <summary>
        /// <see cref="ISportsLeagueService"> instance</see>
        /// </summary>
        private readonly ISportsLeagueService _leagueService;

        /// <summary>
        /// Creates a new <see cref="ScoresParser"/> instance
        /// </summary>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> instance</param>
        public ScoresParser(ISportsLeagueService leagueService)
        {
            this._leagueService = leagueService;
        }

        /// <summary>
        /// Pulls score data from http://www.vegasinsider.com/ for the specified
        /// <see cref="WeekEvent"/>
        /// </summary>
        /// <param name="target">Target <see cref="SeasonWeek"/></param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="WeekEvent"/> objects with updated scores</returns>
        public async Task<List<WeekEvent>> GetScoresForWeekAsync(SeasonWeek target, CancellationToken cancellationToken = default)
        {
            List<WeekEvent> events = new List<WeekEvent>();

            string targetUrl = $"https://www.cbssports.com/nfl/scoreboard/{target.Season.StartDate.Year}/regular/{target.Sequence}/";
            HtmlDocument htmlDoc = await this.WebClient.LoadFromWebAsync(targetUrl, cancellationToken);

            // Get all score tables
            HtmlNodeCollection scoreNodes = htmlDoc
                .DocumentNode
                .SelectNodes("//div[@class='live-update']");

            // Process each result
            foreach (HtmlNode node in scoreNodes)
            {
                // Ensure the game has completed
                // CBS Sports notes this by adding a 'postgame' class to the title
                if (node.SelectSingleNode("descendant::div[@class='game-status postgame']") != null)
                {
                    // Game has completed, get detail
                    HtmlNodeCollection scoreRows = node.SelectNodes("descendant::div[contains(@class, 'in-progress-table')]/table/tbody/tr");

                    if (scoreRows.Count == 2)
                    {
                        // Parse teams
                        string awayTeamName = scoreRows[0].SelectSingleNode("descendant::td/a[contains(@class, 'helper-team-name')]").InnerText;
                        int awayTeamScore = Int32.Parse(scoreRows[0].SelectSingleNode("descendant::td[@class='total-score']").InnerText);
                        string homeTeamName = scoreRows[1].SelectSingleNode("descendant::td/a[contains(@class, 'helper-team-name')]").InnerText;
                        int homeTeamScore = Int32.Parse(scoreRows[1].SelectSingleNode("descendant::td[@class='total-score']").InnerText);

                        if (!String.IsNullOrEmpty(awayTeamName) && !String.IsNullOrEmpty(homeTeamName))
                        {
                            // Get Team objects
                            Team awayTeam = await this._leagueService.GetTeamByNameAsync(awayTeamName, cancellationToken);
                            Team homeTeam = await this._leagueService.GetTeamByNameAsync(homeTeamName, cancellationToken);

                            WeekEvent thisEvent = await this._leagueService
                                .GetWeekEventByWeekAndTeamsAsync(
                                    target.Id,
                                    awayTeam.Id,
                                    homeTeam.Id,
                                    cancellationToken
                                );

                            // Ensure event has not already concluded
                            if (!thisEvent.Completed)
                            {
                                thisEvent.AwayScore = awayTeamScore;
                                thisEvent.HomeScore = homeTeamScore;
                                thisEvent.Completed = true;

                                // Update event
                                events.Add(thisEvent);
                            }
                        }
                    }
                }
            }

            return events;
        }
    }
}
