using FootballShare.Entities.Betting;
using System;

namespace FootballShare.Tasks.Helpers
{
    /// <summary>
    /// <see cref="Exception"/> thrown when attempting to determine payout 
    /// with <see cref="WagerResult.None"/>
    /// </summary>
    public class InvalidWagerResultPayoutException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="WagerResultEmptyException"/> instance
        /// </summary>
        public InvalidWagerResultPayoutException() : base($"Attempted to determine a payout with an unprocessed {nameof(WagerResult)}.")
        {
        }
    }
}
