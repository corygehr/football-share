using Dapper.Contrib.Extensions;
using System;

namespace FootballShare.Entities.Group
{
    /// <summary>
    /// Base Betting <see cref="BettingGroup"/> class
    /// </summary>
    [Table("BettingGroups")]
    public class BettingGroup
    {
        /// <summary>
        /// Unique <see cref="BettingGroup"/> identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// <see cref="BettingGroup"/> is publicly visible
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// <see cref="BettingGroup"/> description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// <see cref="BettingGroup"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Date/Time of <see cref="BettingGroup"/> creation
        /// </summary>
        public DateTimeOffset WhenCreated { get; set; }
    }
}
