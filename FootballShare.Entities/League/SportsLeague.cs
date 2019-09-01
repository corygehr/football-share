using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.League
{
    /// <summary>
    /// Base Sports <see cref="SportsLeague"/> class
    /// </summary>
    [Table("SportsLeagues")]
    public class SportsLeague : Entity
    {
        /// <summary>
        /// <see cref="SportsLeague"/> unique identifier
        /// </summary>
        [ExplicitKey]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="SportsLeague"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="SportsLeague"/> short/common name
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// <see cref="SportsLeague"/> sport
        /// </summary>
        public string SportId { get; set; }

        /// <summary>
        /// <see cref="SportsLeague"/> details
        /// </summary>
        public Sport Sport = null;
    }
}