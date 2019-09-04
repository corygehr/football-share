using FootballShare.Entities.Betting;
using FootballShare.Entities.League;
using FootballShare.Entities.Pools;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// View Model used for placing bets
    /// </summary>
    public class PlaceBetViewModel
    {
        /// <summary>
        /// <see cref="PoolMember"/> object for this <see cref="SiteUser"/>
        /// </summary>
        public PoolMember PoolMembership { get; set; }
        /// <summary>
        /// Target Event <see cref="Spread"/>
        /// </summary>
        public Spread Spread { get; set; }
        /// <summary>
        /// <see cref="Pool"/> ID
        /// </summary>
        public int PoolId { get; set; }
        /// <summary>
        /// Selected <see cref="Team"/> ID
        /// </summary>
        [Display(Name="Team")]
        public string SelectedTeamId { get; set; }
        /// <summary>
        /// Amount to wager
        /// </summary>
        [Display(Name="Wager", Description="Amount to bet.", Prompt="Enter an amount in $100 denoninations.")]
        public decimal WagerAmount { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> ID
        /// </summary>
        public int WeekEventId { get; set; }
    }
}
