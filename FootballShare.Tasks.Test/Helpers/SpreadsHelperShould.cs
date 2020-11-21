using FootballShare.Entities.Betting;
using FootballShare.Tasks.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FootballShare.Tasks.Test.Helpers
{
    /// <summary>
    /// Units Tests for the <see cref="SpreadsHelper"/> class
    /// </summary>
    [TestClass]
    public class SpreadsHelperShould
    {
        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> marks 
        /// a loss if applying a negative spread means the chosen team 
        /// loses the event.
        /// </summary>
        [TestMethod]
        public void ReturnLossIfNegativeSpreadMeansChosenTeamLoses()
        {
            // Arrange
            double chosenSpread = -7.5;
            double chosenScore = 14;
            double opponentScore = 7;
            WagerResult expectedResult = WagerResult.Loss;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> marks 
        /// a loss if applying a positive spread means the chosen team 
        /// loses the event.
        /// </summary>
        [TestMethod]
        public void ReturnLossIfPositiveSpreadMeansChosenTeamLoses()
        {
            // Arrange
            double chosenSpread = 6.5;
            double chosenScore = 14;
            double opponentScore = 21;
            WagerResult expectedResult = WagerResult.Loss;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> marks 
        /// a loss if applying a zero spread means the chosen team 
        /// loses the event.
        /// </summary>
        [TestMethod]
        public void ReturnLossIfZeroSpreadMeansChosenTeamLoses()
        {
            // Arrange
            double chosenSpread = 0;
            double chosenScore = 7;
            double opponentScore = 14;
            WagerResult expectedResult = WagerResult.Loss;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> marks 
        /// a push if applying a negative spread means the chosen team 
        /// ties their opponent.
        /// </summary>
        [TestMethod]
        public void ReturnPushIfNegativeSpreadMeansTeamTied()
        {
            // Arrange
            double chosenSpread = -6;
            double chosenScore = 16;
            double opponentScore = 10;
            WagerResult expectedResult = WagerResult.Push;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> marks 
        /// a push if applying a positive spread means the chosen team 
        /// ties their opponent.
        /// </summary>
        [TestMethod]
        public void ReturnPushIfPositiveSpreadMeansTeamTied()
        {
            // Arrange
            double chosenSpread = 7;
            double chosenScore = 7;
            double opponentScore = 14;
            WagerResult expectedResult = WagerResult.Push;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> marks 
        /// a push if applying a zero spread means the chosen team 
        /// ties their opponent.
        /// </summary>
        [TestMethod]
        public void ReturnLossIfZeroSpreadMeansTeamsTied()
        {
            // Arrange
            double chosenSpread = 0;
            double chosenScore = 21;
            double opponentScore = 21;
            WagerResult expectedResult = WagerResult.Push;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/>  pays out 
        /// if applying a negative spread means the chosen team wins the event.
        /// </summary>
        [TestMethod]
        public void ReturnWinIfNegativeSpreadMeansChosenTeamWins()
        {
            // Arrange
            double chosenSpread = -6.5;
            double chosenScore = 14;
            double opponentScore = 7;
            WagerResult expectedResult = WagerResult.Win;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> pays out 
        /// if applying a positive spread means the chosen team wins the event.
        /// </summary>
        [TestMethod]
        public void ReturnWinIfPositiveSpreadMeansChosenTeamWins()
        {
            // Arrange
            double chosenSpread = 6.5;
            double chosenScore = 7;
            double opponentScore = 13;
            WagerResult expectedResult = WagerResult.Win;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// <see cref="PayoutBetsActivity.DetermineWagerResult()"/> pays out 
        /// if applying a zero spread means the chosen team wins the event.
        /// </summary>
        [TestMethod]
        public void ReturnWinIfZeroSpreadMeansChosenTeamWins()
        {
            // Arrange
            double chosenSpread = 0;
            double chosenScore = 21;
            double opponentScore = 14;
            WagerResult expectedResult = WagerResult.Win;

            // Act
            WagerResult actualResult = SpreadsHelper.DetermineWagerResult(chosenSpread, chosenScore, opponentScore);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
