using Dapper.Contrib.Extensions;
using FootballShare.Entities.User;

using System;

namespace FootballShare.Entities.Group
{
    /// <summary>
    /// <see cref="Group"/> membership class
    /// </summary>
    [Table("GroupMembers")]
    public class GroupMember
    {
        /// <summary>
        /// <see cref="BettingGroup"/> ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> ID
        /// </summary>
        public int SiteUserId { get; set; }
        /// <summary>
        /// Date/Time User joined pool
        /// </summary>
        public DateTimeOffset DateJoined { get; set; }

        /// <summary>
        /// <see cref="BettingGroup"/> details
        /// </summary>
        public BettingGroup Group = null;
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User = null;
    }
}
