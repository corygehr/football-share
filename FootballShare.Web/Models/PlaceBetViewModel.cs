﻿using FootballShare.Entities.Betting;
using FootballShare.Entities.Leagues;
using FootballShare.Entities.Pools;

using System;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// View Model used for placing bets
    /// </summary>
    public class PlaceBetViewModel
    {
        /// <summary>
        /// Target <see cref="WeekEvent"/>
        /// </summary>
        public WeekEvent Event { get; set; }
        /// <summary>
        /// <see cref="PoolMember"/> object for this <see cref="SiteUser"/>
        /// </summary>
        public PoolMember PoolMembership { get; set; }
        /// <summary>
        /// Target Event <see cref="Spread"/>
        /// </summary>
        public Spread Spread { get; set; }
        /// <summary>
        /// <see cref="Pool"/> ID
        /// </summary>
        public int PoolId { get; set; }
        /// <summary>
        /// Selected <see cref="Team"/> ID
        /// </summary>
        [Display(Name="Team")]
        public string SelectedTeamId { get; set; }
        /// <summary>
        /// Amount to wager
        /// </summary>
        [Display(Name="Wager", Description="Amount to bet.", Prompt="Enter an amount in $100 denoninations.")]
        [Range(1, Int32.MaxValue)]
        public decimal WagerAmount { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> ID
        /// </summary>
        public int WeekEventId { get; set; }
    }
}
