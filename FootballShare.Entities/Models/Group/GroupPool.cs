using Dapper.Contrib.Extensions;
using FootballShare.Entities.Models.League;

namespace FootballShare.Entities.Models.Group
{
    /// <summary>
    /// <see cref="Group"/> <see cref="GroupPool"/> class
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
