﻿using FootballShare.Entities.Pools;
using System.Collections.Generic;

namespace FootballShare.Web.Models
{
    public class ListPoolsViewModel
    {
        /// <summary>
        /// <see cref="Pool"/> objects available to join
        /// </summary>
        public IEnumerable<Pool> PublicPools { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> <see cref="Pool"/> memberships
        /// </summary>
        public IEnumerable<PoolMember> UserPools { get; set; }
    }
}
