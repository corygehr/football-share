using Dapper.Contrib.Extensions;

using System;

namespace FootballShare.Entities.User
{
    /// <summary>
    /// Base <see cref="SiteUser"/> class
    /// </summary>
    [Table("SiteUsers")]
    public class SiteUser
    {
        /// <summary>
        /// <see cref="SiteUser"/> unique identifier
        /// </summary>
        /// <remarks>
        /// Some fields are here due to a database dependency (OpenID), but are 
        /// unused by the application itself.
        /// </remarks>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> email address
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> has confirmed they own their listed email address
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> normalized email address
        /// </summary>
        public string NormalizedEmail { get; set; }
        /// <summary>
        /// Normalized <see cref="SiteUser"/> logon name
        /// </summary>
        public string NormalizedUserName { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> Password Hash
        /// </summary>
        /// <remarks>
        /// Should be unused due to reliance on external login providers
        /// </remarks>
        public string PasswordHash { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> Security Stamp string
        /// </summary>
        public string SecurityStamp { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> logon name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Date/Time of <see cref="SiteUser"/> registration
        /// </summary>
        public DateTimeOffset WhenRegistered { get; set; }
    }
}
