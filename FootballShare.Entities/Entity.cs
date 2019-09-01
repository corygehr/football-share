using System;

namespace FootballShare.Entities
{
    /// <summary>
    /// Base <see cref="Entity"/> class
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Date/Time of <see cref="Entity"/> creation
        /// </summary>
        public DateTimeOffset WhenCreated { get; set; }
    }
}
