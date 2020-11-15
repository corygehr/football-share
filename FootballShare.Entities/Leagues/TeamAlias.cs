using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Leagues
{
    /// <summary>
    /// Alternate name for a <see cref="Team"/>
    /// </summary>
    [Table("TeamAliases")]
    public class TeamAlias : EditableEntity
    {
        /// <summary>
        /// Unique <see cref="TeamAlias"/> identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Unique <see cref="Team"/> identifier
        /// </summary>
        public string TeamId { get; set; }
        /// <summary>
        /// Alternate team name
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// <see cref="Team"/> details
        /// </summary>
        public Team Team = null;
    }
}
