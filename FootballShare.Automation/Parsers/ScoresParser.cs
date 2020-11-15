using FootballShare.DAL.Services;
using FootballShare.Entities.Leagues;
using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Automation.Parsers
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

            string targetUrl = $"http://www.vegasinsider.com/nfl/scoreboard/scores.cfm/week/{target.Sequence}/season/{target.Season.StartDate.Year}";

            HtmlDocument htmlDoc = await this.WebClient.LoadFromWebAsync(targetUrl, cancellationToken);

            // Get all score tables
            HtmlNodeCollection scoreNodes = htmlDoc
                .DocumentNode
                .SelectNodes("//table[not(@id) and @class='scoreboardMatchUpContainer']");

            // Process each result
            foreach (HtmlNode node in scoreNodes)
            {
                // Ignore non-HTML nodes (e.g. #text)
                if(node.NodeType == HtmlNodeType.Element && node.Name == "table")
                {
                    // Determine which event this is by its title
                    HtmlNode titleNode = node.SelectSingleNode("descendant::td[@class='yeallowBg2 sportPicksBorderR2 fourleft']");
                    string eventName = titleNode.InnerText;

                    // Parse teams
                    string[] teams = eventName.Split(" @ ");
                    string awayTeamName = teams[0];
                    string homeTeamName = teams[1];

                    // Get team IDs
                    Team awayTeam = await this._leagueService.GetTeamByNameAsync(awayTeamName, cancellationToken);
                    Team homeTeam = await this._leagueService.GetTeamByNameAsync(homeTeamName, cancellationToken);

                    if (awayTeam != null && homeTeam != null)
                    {
                        // Lookup event in current week matching team pattern
                        WeekEvent currentEvent = await this._leagueService
                            .GetWeekEventByWeekAndTeamsAsync(
                            target.Id,
                            awayTeam.Id,
                            homeTeam.Id,
                            cancellationToken
                        );

                        // Get score for event
                        HtmlNodeCollection scores = node.SelectNodes("descendant::td[not(@width)]/font[@color='#b20000']");

                        // Can skip if no score, event may not be over
                        if(scores.Count == 2)
                        {
                            HtmlNode awayScore = scores[0];
                            HtmlNode homeScore = scores[1];

                            currentEvent.AwayScore = Int32.Parse(awayScore.InnerText.Replace("&nbsp;", ""));
                            currentEvent.HomeScore = Int32.Parse(homeScore.InnerText.Replace("&nbsp;", ""));

                            events.Add(currentEvent);
                        }
                    }
                    else
                    {
                        throw new Exception($"Team could not be located or determined for match {eventName}.");
                    }
                }
            }

            return events;
        }
    }
}
