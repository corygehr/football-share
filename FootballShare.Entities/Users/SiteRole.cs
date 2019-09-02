using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Users
{
    /// <summary>
    /// Role definition
    /// </summary>
    [Table("SiteRoles")]
    public class SiteRole : Entity
    {
        /// <summary>
        /// <see cref="SiteRole"/> unique identifier
        /// </summary>
        [ExplicitKey]
        public int Id { get; set; }
        /// <summary>
        /// <see cref="SiteRole"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="SiteRole" normalized name
        /// </summary>
        public string NormalizedName { get; set; }
    }
}
