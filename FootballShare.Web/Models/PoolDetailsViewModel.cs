using FootballShare.Entities.League;
using FootballShare.Entities.Pools;

using System.Collections.Generic;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// Details for viewing a <see cref="Pool"/>
    /// </summary>
    public class PoolDetailsViewModel
    {
        /// <summary>
        /// Next <see cref="SeasonWeek"/> for betting
        /// </summary>
        public SeasonWeek CurrentSeasonWeek { get; set; }
        /// <summary>
        /// Current <see cref="SiteUser"/> <see cref="PoolMember"/> context
        /// </summary>
        public PoolMember CurrentUserMembership { get; set; }
        /// <summary>
        /// <see cref="PoolMember"/> collection
        /// </summary>
        public List<PoolMember> Members { get; set; }
        /// <summary>
        /// <see cref="Pool"/> details
        /// </summary>
        public Pool Pool { get; set; }
        /// <summary>
        /// Previous <see cref="SeasonWeek"/> objects for this <see cref="Pool"/>
        /// </summary>
        public List<SeasonWeek> PreviousSeasonWeeks { get; set; }
    }
}
