using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Models.League
{
    /// <summary>
    /// <see cref="Sport"/> base class
    /// </summary>
    [Table("Sports")]
    public class Sport
    {
        /// <summary>
        /// <see cref="Sport"/> unique identifier
        /// </summary>
        [ExplicitKey]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="Sport"/> name
        /// </summary>
        public string Name { get; set; }
    }
}
