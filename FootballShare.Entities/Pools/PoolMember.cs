using Dapper.Contrib.Extensions;
using FootballShare.Entities.Users;

using System;
using System.ComponentModel.DataAnnotations;

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
        public int Id { get; set; }
        /// <summary>
        /// Available balance ($)
        /// </summary>
        [Display(Name = "Balance", ShortName = "Balance", Description = "Amount each user starts a season with.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Required]
        public decimal Balance { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> is a <see cref="Pool"/> Administrator
        /// </summary>
        [Display(Name = "User is Administrator", ShortName = "Administrator", Description = "If true, user is a pool administrator.")]
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
