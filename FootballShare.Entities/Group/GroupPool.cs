using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;

namespace FootballShare.Entities.Group
{
    /// <summary>
    /// <see cref="BettingGroup"/> <see cref="GroupPool"/> class
    /// </summary>
    [Table("GroupPools")]
    public class GroupPool
    {
        /// <summary>
        /// <see cref="GroupPool"/> unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// <see cref="Season"/> this <see cref="GroupPool"/> is associated with
        /// </summary>
        public string SeasonId { get; set; }

        /// <summary>
        /// <see cref="Season"/> details
        /// </summary>
        public Season Season { get; set; }
    }
}
