using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

namespace FootballShare.Entities.Betting
{
    /// <summary>
    /// <see cref="Wager"/> definition
    /// </summary>
    [Table("Wagers")]
    public class Wager : Entity
    {
        /// <summary>
        /// <see cref="Wager"/> unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
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
        /// <see cref="Pool"/> bet is placed in
        /// </summary>
        public int PoolId { get; set; }
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
        /// <summary>
        /// <see cref="Pool"/> details
        /// </summary>
        public Pool Pool { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User { get; set; }
    }
}
