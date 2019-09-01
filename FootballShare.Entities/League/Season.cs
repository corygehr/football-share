using Dapper.Contrib.Extensions;

using System;

namespace FootballShare.Entities.League
{
    /// <summary>
    /// Base <see cref="League"/> <see cref="Season"/> class
    /// </summary>
    [Table("Seasons")]
    public class Season : Entity
    {
        /// <summary>
        /// <see cref="Season"/> unique identifier
        /// </summary>
        [ExplicitKey]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="Season"/> end date
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// <see cref="League"/> ID
        /// </summary>
        public string LeagueId { get; set; }
        /// <summary>
        /// <see cref="Season"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="Season"/> start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// <see cref="SportsLeague"/> details
        /// </summary>
        public SportsLeague League = null;
    }
}
