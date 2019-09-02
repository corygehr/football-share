using FootballShare.Entities.Group;
using FootballShare.Entities.User;

using System;

namespace FootballShare.Entities.Betting
{
    /// <summary>
    /// <see cref="PoolUserBalance"/> <see cref="SiteUser"/> balance
    /// </summary>
    public class PoolUserBalance : EditableEntity
    {
        /// <summary>
        /// <see cref="SiteUser"/> unique identifier
        /// </summary>
        public Guid SiteUserId { get; set; }
        /// <summary>
        /// <see cref="BettingGroupPool"/> unique identifier
        /// </summary>
        public int BettingGroupPoolId { get; set; }
        /// <summary>
        /// Remaining balance ($)
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// <see cref="BettingGroupPool"/> details
        /// </summary>
        public BettingGroupPool Pool { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User { get; set; } = null;
    }
}
