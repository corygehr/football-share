using FootballShare.Entities.Betting;

namespace FootballShare.Tasks.Helpers
{
    /// <summary>
    /// Helper class for determining bet payouts
    /// </summary>
    public static class PayoutHelper
    {
        /// <summary>
        /// Determines payout amount for a given <see cref="WagerResult"/>
        /// </summary>
        /// <param name="result"><see cref="WagerResult"/> determination</param>
        /// <param name="wagerAmount">Original wager amount</param>
        /// <returns>Payout amount</returns>
        /// <exception cref="InvalidWagerResultPayoutException">
        /// <see cref="WagerResult.None"/> is provided to the function
        /// </exception>
        public static decimal GetPayoutForWagerResult(WagerResult result, decimal wagerAmount)
        {
            switch(result)
            {
                case WagerResult.None:
                    throw new InvalidWagerResultPayoutException();
                case WagerResult.Push:
                case WagerResult.Refund:
                    // Push and Refund return original bet
                    return wagerAmount;
                case WagerResult.Win:
                    // Win doubles amount
                    return wagerAmount * 2;
                default:
                    // Other options return nothing
                    return 0;
            }
        }
    }
}
