using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;

namespace FootballShare.Entities.Group
{
    /// <summary>
    /// <see cref="BettingGroupPool"/> class
    /// </summary>
    [Table("BettingGroupPools")]
    public class BettingGroupPool
    {
        /// <summary>
        /// <see cref="GroupPool"/> unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// <see cref="BettingGroup"/> associated with this <see cref="BettingGroupPool"/>
        /// </summary>
        public int BettingGroupId { get; set; }
        /// <summary>
        /// <see cref="Season"/> this <see cref="BettingGroupPool"/> is associated with
        /// </summary>
        public string SeasonId { get; set; }

        /// <summary>
        /// <see cref="BettingGroup"/> details
        /// </summary>
        public BettingGroup BettingGroup { get; set; }
        /// <summary>
        /// <see cref="Season"/> details
        /// </summary>
        public Season Season { get; set; }
    }
}
