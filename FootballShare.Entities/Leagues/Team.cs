using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Leagues
{
    /// <summary>
    /// Base Sports <see cref="Team"/> class
    /// </summary>
    [Table("Teams")]
    public class Team : EditableEntity
    {
        /// <summary>
        /// Unique <see cref="Team"/> identifier
        /// </summary>
        [ExplicitKey]
        public string Id { get; set; }
        /// <summary>
        /// Affiliated <see cref="League"/> ID
        /// </summary>
        public string LeagueId { get; set; }
        /// <summary>
        /// <see cref="Team"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="Team"/> short/common name
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// <see cref="SportsLeague"/> details
        /// </summary>
        public SportsLeague League = null;
    }
}