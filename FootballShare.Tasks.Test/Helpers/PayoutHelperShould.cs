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
            double originalBet = 100.00;
            double expectedPayout = 200.00;
            WagerResult wagerResult = WagerResult.Win;

            // Act
            double actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

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
            double originalBet = 100.00;
            double expectedPayout = 100.00;
            WagerResult wagerResult = WagerResult.Push;

            // Act
            double actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

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
            double originalBet = 100.00;
            double expectedPayout = 100.00;
            WagerResult wagerResult = WagerResult.Refund;

            // Act
            double actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

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
            double originalBet = 100.00;
            double expectedPayout = 0.00;
            WagerResult wagerResult = WagerResult.Loss;

            // Act
            double actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);

            // Assert
            Assert.AreEqual(expectedPayout, actualPayout);
        }

        /// <summary>
        /// <see cref="PayoutHelper.GetPayoutForWagerResult()"/> throws 
        /// an exception when attempting to process a payout when provided 
        /// with <see cref="WagerResult.None"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidWagerResultPayoutException))]
        public void ThrowExceptionOnNone()
        {
            // Arrange
            double originalBet = 100.00;
            WagerResult wagerResult = WagerResult.None;

            // Act
            // Assert
            Assert.ThrowsException<InvalidWagerResultPayoutException>(() =>
            {
                double actualPayout = PayoutHelper.GetPayoutForWagerResult(wagerResult, originalBet);
            });
        }
    }
}
