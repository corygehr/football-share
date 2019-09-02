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
        public List<SelectListItem> AvailableSeasons { get; set; }
        /// <summary>
        /// <see cref="Pool"/> being created
        /// </summary>
        [Display(Name="Pool Name", ShortName="Name")]
        [Required]
        public string PoolName { get; set; }
        [Display(Name="Season")]
        [Required]
        public string SeasonId { get; set; }
        [Display(Name="Starting Balance ($)", ShortName="Balance ($)", Description = "Amount of money each player starts with.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        [Required]
        public decimal StartingBalance { get; set; } = 10000m;
    }
}
