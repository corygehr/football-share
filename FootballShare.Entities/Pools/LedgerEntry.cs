using FootballShare.Entities.Betting;
using FootballShare.Entities.Users;

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
        public int SiteUserId { get; set; }
        /// <summary>
        /// <see cref="Wager"/> ID
        /// </summary>
        public int WagerId { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> balance before transaction
        /// </summary>
        public decimal StartingBalance { get; set; }
        /// <summary>
        /// Amount of Transaction
        /// </summary>
        public decimal TransactionAmount { get; set; }
        /// <summary>
        /// <see cref="SiteUser"/> balance post-transaction
        /// </summary>
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
