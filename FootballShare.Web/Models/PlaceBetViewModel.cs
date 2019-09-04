using FootballShare.Entities.Betting;
using FootballShare.Entities.League;

using System.ComponentModel.DataAnnotations;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// View Model used for placing bets
    /// </summary>
    public class PlaceBetViewModel
    {
        /// <summary>
        /// Target Event <see cref="Spread"/>
        /// </summary>
        public Spread Spread { get; set; }
        /// <summary>
        /// Selected <see cref="Team"/> ID
        /// </summary>
        [Display(Name="Team")]
        public int SelectedTeamId { get; set; }
        /// <summary>
        /// Amount to wager
        /// </summary>
        [Display(Name="Wager", Description="Amount to bet.", Prompt="Enter an amount in $100 denoninations.")]
        public double WagerAmount { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> ID
        /// </summary>
        public int WeekEventId { get; set; }
    }
}
