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
        public int Id { get; set; }
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
        /// <see cref="SiteUser"/> password hash
        /// </summary>
        /// <remarks>
        /// Unused due to deferrence to OpenID.
        /// </remarks>
        public string PasswordHash { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> phone number
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> has confirmed they own their listed Phone Number
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> has enabled Two-Factor Authentication
        /// </summary>
        /// <remarks>
        /// Unused due to deferrence to OpenID.
        /// </remarks>
        public bool TwoFactorEnabled { get; set; }
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
