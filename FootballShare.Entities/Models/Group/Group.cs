using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Models.Group
{
    /// <summary>
    /// Base Betting <see cref="Group"/> class
    /// </summary>
    [Table("Groups")]
    public class Group
    {
        /// <summary>
        /// Unique <see cref="Group"/> identifier
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="Group"/> description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// <see cref="Group"/> name
        /// </summary>
        public string Name { get; set; }
    }
}
