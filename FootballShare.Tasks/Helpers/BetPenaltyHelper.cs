using FootballShare.DAL.Services;

namespace FootballShare.Tasks.Helpers
{
    /// <summary>
    /// Determines <see cref="PoolMember"/> penalties for failing to make minimum <see cref="Wager"/> quotas
    /// </summary>
    public class BetPenaltyHelper
    {
        /// <summary>
        /// <see cref="ISportsLeagueService"/> implemention for event retrieval
        /// </summary>
        private ISportsLeagueService _leagueService;
        /// <summary>
        /// <see cref="IPoolService"/> implementation for <see cref="Pool"/> management
        /// </summary>
        private IPoolService _poolService;

        /// <summary>
        /// Creates a new <see cref="BetPenaltyHelper"/> instance
        /// </summary>
        /// <param name="leagueService"><see cref="ISportsLeagueService"/> implementation used for retrieving event data</param>
        /// <param name="poolService"><see cref="IPoolService"/> implementation for <see cref="Pool"/> management</param>
        public BetPenaltyHelper(ISportsLeagueService leagueService, IPoolService poolService)
        {
            this._leagueService = leagueService;
            this._poolService = poolService;
        }
    }
}