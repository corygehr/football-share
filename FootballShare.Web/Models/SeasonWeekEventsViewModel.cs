using FootballShare.Entities.Betting;
using FootballShare.Entities.League;

using System.Collections.Generic;

namespace FootballShare.Web.Models
{
    public class SeasonWeekEventsViewModel
    {
        /// <summary>
        /// <see cref="SeasonWeek"/> events
        /// </summary>
        public List<Spread> EventSpreads { get; set; }
    }
}
