using FootballShare.Entities.Leagues;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Services
{
    /// <summary>
    /// <see cref="SportsLeague"/> Service interface
    /// </summary>
    public interface ISportsLeagueService : IService
    {
        /// <summary>
        /// Gets all current <see cref="Season"/> objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="Season"/> objects</returns>
        Task<IEnumerable<Season>> GetAllCurrentSeasonsAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the specified <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId"><see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Requested <see cref="SportsLeague"/></returns>
        Task<SportsLeague> GetLeagueAsync(string leagueId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the current <see cref="Season"/> for the specified <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId">Requested <see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Current <see cref="SportsLeague"/> <see cref="Season"/></returns>
        Task<Season> GetLeagueCurrentSeasonAsync(string leagueId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the previous <see cref="SeasonWeek"/> for the specified <see cref="SportsLeague"/>
        /// </summary>
        /// <param name="leagueId">Requested <see cref="SportsLeague"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Previous <see cref="SportsLeague"/> <see cref="SeasonWeek"/></returns>
        Task<SeasonWeek> GetLeaguePreviousWeekAsync(string leagueId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId"><see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="Season"/> object</returns>
        Task<Season> GetSeasonAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="WeekEvent"/> objects for the specified <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId">Target <see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="WeekEvent"/> objects</returns>
        Task<IEnumerable<WeekEvent>> GetSeasonEventsAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets all <see cref="SeasonWeek"/> objects for a specific <see cref="Season"/>
        /// </summary>
        /// <param name="seasonId">Target <see cref="Season"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of <see cref="SeasonWeek"/> objects</returns>
        Task<IEnumerable<SeasonWeek>> GetSeasonWeeksAsync(string seasonId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the requested <see cref="Team"/>
        /// </summary>
        /// <param name="teamId"><see cref="Team"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="Team"/> or <see cref="null"/></returns>
        Task<Team> GetTeamAsync(string teamId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the requested <see cref="Team"/> by its name
        /// </summary>
        /// <param name="teamName"><see cref="Team"/> name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="Team"/> or <see cref="null"/></returns>
        Task<Team> GetTeamByNameAsync(string teamName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the requested <see cref="WeekEvent"/>
        /// </summary>
        /// <param name="eventId"><see cref="WeekEvent"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="WeekEvent"/> or null if not found</returns>
        Task<WeekEvent> GetWeekEventAsync(int eventId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets a <see cref="WeekEvent"/> based on <see cref="Team"/> playing and <see cref="SeasonWeek"/> ID
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="awayTeamId">Away <see cref="Team"/> ID</param>
        /// <param name="homeTeamId">Home <see cref="Team"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="WeekEvent"/> or <see cref="null"/></returns>
        Task<WeekEvent> GetWeekEventByWeekAndTeamsAsync(string weekId, string awayTeamId, string homeTeamId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Updates the score for a specific <see cref="WeekEvent"/>
        /// </summary>
        /// <param name="eventId"><see cref="WeekEvent"/> ID</param>
        /// <param name="awayScore">Away <see cref="Team"/> score</param>
        /// <param name="homeScore">Home <see cref="Team"/> score</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateEventScoreAsync(int eventId, int awayScore, int homeScore, CancellationToken cancellationToken = default);
    }
}
