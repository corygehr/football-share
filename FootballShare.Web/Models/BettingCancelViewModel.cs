using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// View Model for Betting/Cancel
    /// </summary>
    public class BettingCancelViewModel
    {
        /// <summary>
        /// Target <see cref="WeekEvent"/>
        /// </summary>
        public WeekEvent Event { get; set; }
        /// <summary>
        /// Target <see cref="Wager"/>
        /// </summary>
        public Wager Wager { get; set; }
    }
}
