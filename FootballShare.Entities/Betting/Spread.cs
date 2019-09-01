using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;

namespace FootballShare.Entities.Betting
{
    /// <summary>
    /// <see cref="Spread"/> class
    /// </summary>
    [Table("Spreads")]
    public class Spread : Entity
    {
        /// <summary>
        /// <see cref="Spread"/> unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Away Team Spread
        /// </summary>
        public double AwaySpread { get; set; }
        /// <summary>
        /// Home Team Spread
        /// </summary>
        public double HomeSpread { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> for this <see cref="Spread"/>
        /// </summary>
        public string WeekEventId { get; set; }

        /// <summary>
        /// <see cref="WeekEvent"/> details
        /// </summary>
        public WeekEvent Event { get; set; } = null;
    }
}
