using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

using System;

namespace FootballShare.Entities.User
{
    /// <summary>
    /// Base <see cref="SiteUser"/> class
    /// </summary>
    [Table("SiteUsers")]
    public class SiteUser : EditableEntity
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
        /// <see cref="SiteUser"/> display name
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> email address
        /// </summary>
        [PersonalData]
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
        [PersonalData]
        public string NormalizedEmail { get; set; }
        /// <summary>
        /// Normalized <see cref="SiteUser"/> logon name
        /// </summary>
        public string NormalizedUserName { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> logon name
        /// </summary>
        public string UserName { get; set; }
    }
}
