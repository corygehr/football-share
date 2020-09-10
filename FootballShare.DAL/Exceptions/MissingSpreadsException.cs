using System;

namespace FootballShare.DAL.Exceptions
{
    public class MissingSpreadsException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="MissingSpreadsException"/> instance
        /// </summary>
        public MissingSpreadsException() : 
            base($"One or more events for this week do not have spreads.")
        {

        }
    }
}
