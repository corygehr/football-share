using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;

namespace FootballShare.Entities.Betting
{
    /// <summary>
    /// <see cref="Wager"/> definition
    /// </summary>
    [Table("Wagers")]
    public class Wager : Entity
    {
        /// <summary>
        /// <see cref="Wager"/> amount
        /// </summary>
        public double Amount { get; set; }
        /// <summary>
        /// Away Team <see cref="Spread"/> at time of wager
        /// </summary>
        public double AwaySpread { get; set; }
        /// <summary>
        /// Home Team <see cref="Spread"/> at time of wager
        /// </summary>
        public double HomeSpread { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> placing bet
        /// </summary>
        public string SiteUserId { get; set; }
        /// <summary>
        /// Team selected (Home/Away)
        /// </summary>
        public WagerTarget Target { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> of bet
        /// </summary>
        public int WeekEventId { get; set; }

        /// <summary>
        /// <see cref="WeekEvent"/> details
        /// </summary>
        public WeekEvent Event { get; set; }
    }
}
