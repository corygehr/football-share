using System;

namespace FootballShare.Entities
{
    /// <summary>
    /// Base class for editable <see cref="Entity"/> objects
    /// </summary>
    public abstract class EditableEntity : Entity
    {
        /// <summary>
        /// Date/Time of last <see cref="Entity"/> update
        /// </summary>
        public DateTimeOffset WhenUpdated { get; set; }
    }
}