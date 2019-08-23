using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

using System;

namespace FootballShare.Entities.User
{
    /// <summary>
    /// Base <see cref="SiteUser"/> class
    /// </summary>
    [Table("SiteUsers")]
    public class SiteUser : IdentityUser
    {
        /// <summary>
        /// <see cref="SiteUser"/> name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Date/Time of <see cref="SiteUser"/> registration
        /// </summary>
        public DateTimeOffset WhenRegistered { get; set; }
        /// <summary>
        /// Date/Time of last <see cref="SiteUser"/> update
        /// </summary>
        public DateTimeOffset WhenUpdated { get; set; }
    }
}
