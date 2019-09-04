using System;

namespace FootballShare.DAL.Exceptions
{
    public class MaxBetsPlacedForWeekException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="MaxBetsPlacedForWeekException"/> instance
        /// </summary>
        /// <param name="maxBets">Maximum bets allowed to be placed for the week</param>
        public MaxBetsPlacedForWeekException(int maxBets) : 
            base($"Each user can only place {maxBets} bet(s) per week.")
        {

        }
    }
}
