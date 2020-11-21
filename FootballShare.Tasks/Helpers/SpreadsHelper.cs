using FootballShare.Entities.Betting;

namespace FootballShare.Tasks.Helpers
{
    /// <summary>
    /// Helper class for calculating winners based on Spreads
    /// </summary>
    public static class SpreadsHelper
    {
        /// <summary>
        /// Determines the result of a bet using spreads
        /// </summary>
        /// <param name="wagerSpread">Spread when wager was created</param>
        /// <param name="selectedTeamScore">Selected team score</param>
        /// <param name="opponentScore">Opposing team score</param>
        /// <returns><see cref="WagerResult"/> determination</returns>
        public static WagerResult DetermineWagerResult(double wagerSpread, double selectedTeamScore, double opponentScore)
        {
            // Spreads are essentially a handicap
            double chosenTeamScoreWithSpread = selectedTeamScore + wagerSpread;

            if (chosenTeamScoreWithSpread > opponentScore)
            {
                // If the selected team wins after the spread is applied, the bet pays out
                return WagerResult.Win;
            }
            else if (chosenTeamScoreWithSpread == opponentScore)
            {
                // If the selected team ties after the spread is applied, the bet pushes
                return WagerResult.Push;
            }
            else
            {
                // If the selected team ties after the spread is applied, the bet loses
                return WagerResult.Loss;
            }
        }
    }
}
