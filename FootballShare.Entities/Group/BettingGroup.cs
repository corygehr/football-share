using Dapper.Contrib.Extensions;

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
        public string Id { get; set; }
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
