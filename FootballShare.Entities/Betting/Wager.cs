using Dapper.Contrib.Extensions;
using FootballShare.Entities.League;
using FootballShare.Entities.Pools;
using FootballShare.Entities.Users;

using System;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Entities.Betting
{
    /// <summary>
    /// <see cref="Wager"/> definition
    /// </summary>
    [Table("Wagers")]
    public class Wager : Entity
    {
        /// <summary>
        /// <see cref="Wager"/> unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// <see cref="Wager"/> amount
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// <see cref="Pool"/> bet is placed in
        /// </summary>
        public int PoolId { get; set; }
        /// <summary>
        /// Selected <see cref="Team"/> ID
        /// </summary>
        public string SelectedTeamId { get; set; }
        /// <summary>
        /// Spread for selected <see cref="Team"/>
        /// </summary>
        public double SelectedTeamSpread { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> placing bet
        /// </summary>
        public Guid SiteUserId { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> of bet
        /// </summary>
        public int WeekEventId { get; set; }

        /// <summary>
        /// <see cref="WeekEvent"/> details
        /// </summary>
        public WeekEvent Event { get; set; }
        /// <summary>
        /// <see cref="Pool"/> details
        /// </summary>
        public Pool Pool { get; set; }
        /// <summary>
        /// <see cref="Team"/> details
        /// </summary>
        public Team SelectedTeam { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User { get; set; }
    }
}
