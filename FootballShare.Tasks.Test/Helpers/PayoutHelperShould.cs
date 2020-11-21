using FootballShare.Entities.Betting;
using FootballShare.Tasks.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FootballShare.Tasks.Test.Helpers
{
    /// <summary>
    /// Unit tests for the <see cref="PayoutHelper"/> class
    /// </summary>
    [TestClass]
    public class PayoutHelperShould
    {
        /// <summary>
        /// <see cref="PayoutHelper.GetPayoutForWagerResult()"/> returns 
        /// double the original bet amount.
        /// </summary>
        [TestMethod]
        public void ReturnDoubleOriginalBetOnWin()
        {
            // Arrange
            decimal originalBet = 100;
            decimal expectedPayout = 200;
            WagerResult wagerResult = WagerResult.Win;

            // Act
            decimal actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

            // Assert
            Assert.AreEqual(expectedPayout, actualPayout);
        }

        /// <summary>
        /// <see cref="PayoutHelper.GetPayoutForWagerResult()"/> returns 
        /// original bet amount on <see cref="WagerResult.Push"/>.
        /// </summary>
        [TestMethod]
        public void ReturnOriginalBetOnPush()
        {
            // Arrange
            decimal originalBet = 100;
            decimal expectedPayout = 100;
            WagerResult wagerResult = WagerResult.Push;

            // Act
            decimal actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

            // Assert
            Assert.AreEqual(expectedPayout, actualPayout);
        }

        /// <summary>
        /// <see cref="PayoutHelper.GetPayoutForWagerResult()"/> returns 
        /// original bet amount on <see cref="WagerResult.Refund"/>.
        /// </summary>
        [TestMethod]
        public void ReturnOriginalBetOnRefund()
        {
            // Arrange
            decimal originalBet = 100;
            decimal expectedPayout = 100;
            WagerResult wagerResult = WagerResult.Refund;

            // Act
            decimal actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

            // Assert
            Assert.AreEqual(expectedPayout, actualPayout);
        }

        /// <summary>
        /// <see cref="PayoutHelper.GetPayoutForWagerResult()"/> returns 
        /// zero on a loss.
        /// </summary>
        [TestMethod]
        public void ReturnZeroOnLoss()
        {
            // Arrange
            decimal originalBet = 100;
            decimal expectedPayout = 0;
            WagerResult wagerResult = WagerResult.Loss;

            // Act
            decimal actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

            // Assert
            Assert.AreEqual(expectedPayout, actualPayout);
        }

        /// <summary>
        /// <see cref="PayoutHelper.GetPayoutForWagerResult()"/> throws 
        /// an exception when attempting to process a payout when provided 
        /// with <see cref="WagerResult.None"/>.
        /// </summary>
        [TestMethod]
        public void ThrowExceptionOnNone()
        {
            // Arrange
            decimal originalBet = 100;
            WagerResult wagerResult = WagerResult.None;

            // Act
            // Assert
            Assert.ThrowsException<InvalidWagerResultPayoutException>(() =>
            {
                decimal actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);
            });
        }
    }
}
