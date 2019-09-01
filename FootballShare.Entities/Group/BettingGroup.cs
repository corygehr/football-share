using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Group
{
    /// <summary>
    /// Base Betting <see cref="BettingGroup"/> class
    /// </summary>
    [Table("BettingGroups")]
    public class BettingGroup : EditableEntity
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
    }
}
