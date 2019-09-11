using FootballShare.Entities.Betting;
using FootballShare.Entities.Pools;

using System.Collections.Generic;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// View Model used for the 'History' action in <see cref="BettingController"/>
    /// </summary>
    public class BettingHistoryViewModel
    {
        /// <summary>
        /// <see cref="Pool"/> details
        /// </summary>
        public Pool Pool { get; set; } = null;
        /// <summary>
        /// <see cref="Wager"/> objects for the <see cref="SeasonWeek"/>
        /// </summary>
        public List<Wager> Wagers { get; set; } = new List<Wager>();
    }
}
