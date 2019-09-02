using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;

namespace FootballShare.Entities.Pools
{
    /// <summary>
    /// <see cref="Pool"/> class
    /// </summary>
    [Table("Pools")]
    public class Pool : EditableEntity
    {
        /// <summary>
        /// <see cref="Pool"/> unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// <see cref="Pool"/> is publicly available
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// <see cref="Pool"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="Season"/> this <see cref="BettingGroupPool"/> is associated with
        /// </summary>
        public string SeasonId { get; set; }
        /// <summary>
        /// Amount each <see cref="PoolMember"/> starts with
        /// </summary>
        public double StartingBalance { get; set; }

        /// <summary>
        /// <see cref="Season"/> details
        /// </summary>
        public Season Season { get; set; }
    }
}
