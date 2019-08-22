using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Models.User
{
    /// <summary>
    /// Base <see cref="User"/> class
    /// </summary>
    [Table("Users")]
    public class User
    {
        /// <summary>
        /// <see cref="User"/> unique identifier
        /// </summary>
        [Key]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="User"/> email address
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// <see cref="User"/> name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// <see cref="User"/> alias
        /// </summary>
        public string Username { get; set; }
    }
}
