using Dapper.Contrib.Extensions;

namespace FootballShare.Entities.Models.League
{
    /// <summary>
    /// Base Sports <see cref="League"/> class
    /// </summary>
    [Table("Leagues")]
    public class League
    {
        /// <summary>
        /// <see cref="League"/> unique identifier
        /// </summary>
        [ExplicitKey]
        public string Id { get; set; }
        /// <summary>
        /// <see cref="League"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="League"/> short/common name
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// <see cref="League"/> sport
        /// </summary>
        public string SportId { get; set; }

        /// <summary>
        /// <see cref="Sport"/> details
        /// </summary>
        public Sport Sport = null;
    }
}
