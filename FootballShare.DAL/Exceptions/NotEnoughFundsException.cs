using System;

namespace FootballShare.DAL.Exceptions
{
    /// <summary>
    /// Thrown when the user does not have enough funds to place a bet
    /// </summary>
    public class NotEnoughFundsException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="NotEnoughFundsException"/>
        /// </summary>
        /// <param name="requiredAmount">Required funds for transaction</param>
        /// <param name="userFunds">Current user funds</param>
        public NotEnoughFundsException(double requiredAmount, double userFunds)
            : base($"Not enough funds to make transaction (need at least ${requiredAmount}); user has ${userFunds}.")
        {

        }
    }
}
