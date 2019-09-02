using Dapper.Contrib.Extensions;
using FootballShare.Entities.Users;

using System;

namespace FootballShare.Entities.Pools
{
    /// <summary>
    /// <see cref="Pool"/> membership class
    /// </summary>
    [Table("PoolMembers")]
    public class PoolMember : EditableEntity
    {
        /// <summary>
        /// <see cref="PoolMember"/> unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Available balance ($)
        /// </summary>
        public double Balance { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> is a <see cref="Pool"/> Administrator
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// <see cref="Pool"/> ID
        /// </summary>
        public int PoolId { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> ID
        /// </summary>
        public Guid SiteUserId { get; set; }

        /// <summary>
        /// <see cref="Pool"/> details
        /// </summary>
        public Pool Pool = null;
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User = null;
    }
}
