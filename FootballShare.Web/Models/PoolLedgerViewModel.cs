using FootballShare.Entities.Pools;

using System.Collections.Generic;

namespace FootballShare.Web.Models
{
    /// <summary>
    /// View Model used when displaying a Pool's ledger
    /// </summary>
    public class PoolLedgerViewModel
    {
        /// <summary>
        /// <see cref="LedgerEntry"/> collection
        /// </summary>
        public List<LedgerEntry> Ledger { get; set; } = new List<LedgerEntry>();
        /// <summary>
        /// <see cref="Pool"/> data
        /// </summary>
        public Pool Pool { get; set; }
    }
}
