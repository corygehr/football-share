using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Entities.Pools;

using System.Collections.Generic;

namespace FootballShare.Web.Models
{
    public class SeasonWeekEventsViewModel
    {
        /// <summary>
        /// <see cref="SeasonWeek"/> events
        /// </summary>
        public List<Spread> EventSpreads { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> <see cref="PoolMember"/> instance
        /// </summary>
        public PoolMember PoolMembership { get; set; }
        /// <summary>
        /// <see cref="Wager"/> objects for user in the <see cref="SeasonWeek"/>
        /// </summary>
        public List<Wager> WeekWagers { get; set; } = new List<Wager>();
    }
}
