using Dapper.Contrib.Extensions;

using System;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Entities.League
{
    /// <summary>
    /// Base <see cref="League"/> <see cref="Season"/> class
    /// </summary>
    [Table("Seasons")]
    public class Season : Entity
    {
        /// <summary>
        /// <see cref="Season"/> unique identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// <see cref="Season"/> end date
        /// </summary>
        [Display(Name = "Season End Date", ShortName = "End Date", Description = "Date season ends.")]
        [Required]
        public DateTimeOffset EndDate { get; set; }
        /// <summary>
        /// <see cref="Season"/> name
        /// </summary>
        [Display(Name = "Season", ShortName = "Name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// <see cref="SportsLeague"/> ID
        /// </summary>
        public string SportsLeagueId { get; set; }
        /// <summary>
        /// <see cref="Season"/> start date
        /// </summary>
        [Display(Name = "Season End Date", ShortName = "Start Date", Description = "Date season begins.")]
        [Required]
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// <see cref="SportsLeague"/> details
        /// </summary>
        public SportsLeague League = null;
    }
}
