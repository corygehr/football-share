using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;

using System.ComponentModel.DataAnnotations;

namespace FootballShare.Entities.Pools
{
    /// <summary>
    /// <see cref="Pool"/> class
    /// </summary>
    [Table("Pools")]
    public class Pool : EditableEntity
    {
        /// <summary>
        /// <see cref="Pool"/> unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// <see cref="Pool"/> is publicly available
        /// </summary>
        [Display(Name = "Open to Public?", ShortName = "Public", Description = "Anyone from the general public can join this pool.")]
        public bool IsPublic { get; set; }
        /// <summary>
        /// <see cref="Pool"/> name
        /// </summary>
        [Display(Name = "Name", Description = "Pool Name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// <see cref="Season"/> this <see cref="BettingGroupPool"/> is associated with
        /// </summary>
        public string SeasonId { get; set; }
        /// <summary>
        /// Amount each <see cref="PoolMember"/> starts with
        /// </summary>
        [Display(Name = "Starting Balance", ShortName = "Start Balance", Description = "Amount each user starts a season with.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Required]
        public decimal StartingBalance { get; set; }

        /// <summary>
        /// <see cref="Season"/> details
        /// </summary>
        public Season Season { get; set; }
    }
}
