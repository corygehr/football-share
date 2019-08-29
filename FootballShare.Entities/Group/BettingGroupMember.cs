using Dapper.Contrib.Extensions;
using FootballShare.Entities.User;

using System;

namespace FootballShare.Entities.Group
{
    /// <summary>
    /// <see cref="BettingGroupMember"/> membership class
    /// </summary>
    [Table("BettingGroupMembers")]
    public class BettingGroupMember
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
        /// <see cref="SiteUser"/> is a <see cref="BettingGroup"/> Administrator
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// Date/Time User joined pool
        /// </summary>
        public DateTimeOffset WhenJoined { get; set; }

        /// <summary>
        /// <see cref="BettingGroup"/> details
        /// </summary>
        public BettingGroup BettingGroup = null;
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User = null;
    }
}
