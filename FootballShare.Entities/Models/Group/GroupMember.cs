using System;

namespace FootballShare.Entities.Models.Group
{
    /// <summary>
    /// <see cref="Group"/> membership class
    /// </summary>
    public class GroupMember
    {
        /// <summary>
        /// <see cref="Group"/> ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Date/Time User joined pool
        /// </summary>
        public DateTimeOffset DateJoined { get; set; }
    }
}
