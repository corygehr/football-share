using Dapper.Contrib.Extensions;

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
        public string ExternalLoginId { get; set; }
        /// <summary>
        /// External Provider name
        /// </summary>
        public string LoginProviderName { get; set; }
        /// <summary>
        /// External Provider key
        /// </summary>
        public string ProviderKey { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> unique identifier
        /// </summary>
        public string UserId { get; set; }
    }
}
