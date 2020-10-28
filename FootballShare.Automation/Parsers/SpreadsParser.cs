using FootballShare.DAL.Repositories;
using FootballShare.DAL.Services;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using HtmlAgilityPack;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.Automation.Parsers
{
    /// <summary>
    /// Pulls <see cref="SpreadsParser"/> objects from Vegas Insider
    /// </summary>
    public class SpreadsParser : HtmlParser
    {
        /// <summary>
        /// Betting Service class
        /// </summary>
        private readonly IBettingService _bettingService;
        /// <summary>
        /// Sports League Service class
        /// </summary>
        private readonly ISportsLeagueService _leagueService;

        /// <summary>
        /// Creates a new <see cref="SpreadsParser"/> instance
        /// </summary>
        /// <param name="bettingService">Betting service</param>
        /// <param name="leagueService">Sports League service</param>
        public SpreadsParser(IBettingService bettingService, ISportsLeagueService leagueService)
        {
            this._bettingService = bettingService;
            this._leagueService = leagueService;
        }

        /// <summary>
        /// Pulls <see cref="Spread"/> data from http://www.vegasinsider.com/ for the specified 
        /// <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Spread"/> objects</returns>
        public async Task<List<Spread>> GetSpreadsForWeekAsync(SeasonWeek target, CancellationToken cancellationToken = default)
        {
            List<Spread> events = new List<Spread>();

            string targetUrl = $"http://www.vegasinsider.com/nfl/matchups/matchups.cfm/week/{target.Sequence}/season/{target.Season.StartDate.Year}";

            HtmlDocument htmlDoc = await this.WebClient.LoadFromWebAsync(targetUrl, cancellationToken);

            HtmlNode scheduleNode = htmlDoc
                .DocumentNode
                .SelectSingleNode("//td[@class='main-content-cell']/div[not(@id) and @class='SLTables1']");

            DateTimeOffset currentEventDate = DateTimeOffset.UtcNow;

            // Get events for a specific day
            foreach(HtmlNode node in scheduleNode.ChildNodes)
            {
                // Ignore non-HTML nodes (e.g. #text)
                if(node.NodeType == HtmlNodeType.Element)
                {
                    if(node.Name == "div" && node.Attributes.Any(a => a.Name == "class" && a.Value == "SLTables1"))
                    {
                        // Get title (which has team names)
                        HtmlNode titleNode = node.SelectSingleNode("./table/tr/td[@class='viHeaderNorm']");
                        string eventName = titleNode.FirstChild.GetDirectInnerText().Replace("\r", "").Replace("\n", "").Replace("\t", "");

                        // Parse teams
                        string[] teams = eventName.Split(" @ ");
                        string awayTeamName = teams[0];
                        string homeTeamName = teams[1];

                        // Get team IDs
                        Team awayTeam = await this._leagueService.GetTeamByNameAsync(awayTeamName, cancellationToken);
                        Team homeTeam = await this._leagueService.GetTeamByNameAsync(homeTeamName, cancellationToken);

                        // Confirm we have the teams for this matchup
                        if(awayTeam != null && homeTeam != null)
                        {
                            // Get event for playing teams
                            WeekEvent associatedEvent = await this._leagueService.GetWeekEventByWeekAndTeamsAsync(target.Id, awayTeam.Id, homeTeam.Id, cancellationToken);

                            // Ensure there's a matching event in the database
                            // For now, skip if there isn't since the local database is more accurate (usually)
                            if(associatedEvent != null)
                            {
                                // Get current spread object for playing team
                                Spread currentSpread = await this._bettingService.GetSpreadForEventAsync(associatedEvent.Id, cancellationToken);

                                // Parse spread from site
                                HtmlNodeCollection spreadNodes = node.SelectNodes("./table/tr/td[@class='viBodyBorderNorm']/table/tr/td[contains(@class, 'viCellBg2')]");

                                // These are static - it's hard to determine this via context clues
                                HtmlNode awaySpreadNode = spreadNodes[4];
                                HtmlNode homeSpreadNode = spreadNodes[12];

                                string[] evenIndicators = new string[]
                                {
                                "&nbsp;",
                                "",
                                "PK"
                                };

                                // Check for "Even" or empty
                                if (evenIndicators.Contains(awaySpreadNode.InnerText) || evenIndicators.Contains(homeSpreadNode.InnerText))
                                {
                                    // Even or null spread
                                    currentSpread.AwaySpread = 0;
                                    currentSpread.HomeSpread = 0;
                                }
                                else
                                {
                                    double awaySpread = Double.Parse(awaySpreadNode.InnerText);
                                    double homeSpread = Double.Parse(homeSpreadNode.InnerText);

                                    // Smaller value is best indicator of spread for how this application checks it
                                    double usableSpread = Math.Min(awaySpread, homeSpread);

                                    if (usableSpread == awaySpread)
                                    {
                                        currentSpread.AwaySpread = awaySpread;
                                        currentSpread.HomeSpread = -awaySpread;
                                    }
                                    else
                                    {
                                        currentSpread.AwaySpread = -homeSpread;
                                        currentSpread.HomeSpread = homeSpread;
                                    }
                                }

                                // Store new spread
                                events.Add(currentSpread);
                            }
                        }
                        else
                        {
                            throw new Exception($"Team could not be located for match {eventName}.");
                        }
                    }
                }
            }

            return events;
        }
    }
}
