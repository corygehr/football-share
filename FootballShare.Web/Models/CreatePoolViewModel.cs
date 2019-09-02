using FootballShare.Entities.League;
using FootballShare.Entities.Pools;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Web.Models
{
    public class CreatePoolViewModel
    {
        /// <summary>
        /// Available <see cref="Season"/> objects for <see cref="Pool"/> creation
        /// </summary>
        public IEnumerable<SelectListItem> AvailableSeasons { get; set; }
        /// <summary>
        /// <see cref="Pool"/> being created
        /// </summary>
        [Display(Name="Pool")]
        [Required]
        public string PoolName { get; set; }
        [Display(Name="Season")]
        [Required]
        public string SeasonId { get; set; }
        [Display(Name="Starting Balance ($)", ShortName="Balance ($)", Description = "Amount of money each player starts with.")]
        [Required]
        public double StartingBalance { get; set; } = 10000.00;
    }
}
