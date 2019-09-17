using FootballShare.DAL.Repositories;
using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using HtmlAgilityPack;

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
        /// <see cref="Team"/> repository
        /// </summary>
        private readonly ITeamRepository _teamRepo;

        /// <summary>
        /// Creates a new <see cref="SpreadsParser"/> instance
        /// </summary>
        /// <param name="teamRepo"><see cref="Team"/> repository</param>
        public SpreadsParser(ITeamRepository teamRepo)
        {
            this._teamRepo = teamRepo;
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
                        Team awayTeam = await this._teamRepo.GetByNameAsync(awayTeamName, cancellationToken);
                        Team homeTeam = await this._teamRepo.GetByNameAsync(homeTeamName, cancellationToken);

                        // Get game time
                        HtmlNode timeNode = node.SelectSingleNode("./table/tr/td[@class='viBodyBorderNorm']/table/tr/td[contains(@class, 'viSubHeader1')]");
                        string eventTimeRaw = timeNode.FirstChild.GetDirectInnerText().Replace(" Game Time", "");

                        DateTime eventTime = DateTime.Parse(eventTimeRaw);
                        DateTimeOffset eventTimeFull = currentEventDate.AddHours(eventTime.Hour).AddMinutes(eventTime.Minute);

                        if(awayTeam != null && homeTeam != null)
                        {

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
