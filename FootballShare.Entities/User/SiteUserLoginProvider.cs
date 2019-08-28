using Dapper.Contrib.Extensions;
using System;

namespace FootballShare.Entities.User
{
    /// <summary>
    /// Pairs a <see cref="SiteUser"/> to an External Login provider
    /// </summary>
    [Table("SiteUserLoginProviders")]
    public class SiteUserLoginProvider
    {
        /// <summary>
        /// External Login unique identifier
        /// </summary>
        [Key]
        public Guid ExternalLoginId { get; set; }
        /// <summary>
        /// External Provider name
        /// </summary>
        public string LoginProvider { get; set; }
        /// <summary>
        /// External Provider display name
        /// </summary>
        public string ProviderDisplayName { get; set; }
        /// <summary>
        /// External Provider key
        /// </summary>
        public string ProviderKey { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> unique identifier
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// <see cref="SiteUserLoginProvider"/> registration date/time
        /// </summary>
        public DateTimeOffset WhenRegistered { get; set; }
    }
}
