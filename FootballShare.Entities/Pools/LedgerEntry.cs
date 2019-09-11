using FootballShare.Entities.Betting;
using FootballShare.Entities.Users;

using System;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Entities.Pools
{
    /// <summary>
    /// <see cref="LedgerEntry"/> class
    /// </summary>
    public class LedgerEntry : Entity
    {
        /// <summary>
        /// <see cref="LedgerEntry"/> unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Transaction description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// <see cref="Pool"/> ID
        /// </summary>
        public int PoolId { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> ID
        /// </summary>
        public Guid SiteUserId { get; set; }
        /// <summary>
        /// <see cref="Wager"/> ID
        /// </summary>
        public int WagerId { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> balance before transaction
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal StartingBalance { get; set; }
        /// <summary>
        /// Amount of Transaction
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> balance post-transaction
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal NewBalance { get; set; }

        /// <summary>
        /// <see cref="Pool"/> details
        /// </summary>
        public Pool Pool { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> details
        /// </summary>
        public SiteUser User { get; set; }
        /// <summary>
        /// <see cref="Wager"/> details
        /// </summary>
        public Wager Wager { get; set; }
    }
}
