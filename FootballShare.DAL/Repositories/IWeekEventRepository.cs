using FootballShare.Entities.Leagues;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FootballShare.DAL.Repositories
{
    /// <summary>
    /// Defines the <see cref="WeekEvent"/> repository interface
    /// </summary>
    public interface IWeekEventRepository : IRepository<WeekEvent>
    {
        /// <summary>
        /// Retrieves a <see cref="WeekEvent"/> object by its associated <see cref="SeasonWeek"/> and involved <see cref="Team"/> objects
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="awayTeamId">Away <see cref="Team"/> ID</param>
        /// <param name="homeTeamId">Home <see cref="Team"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Matching <see cref="WeekEvent"/> or <see cref="null"/></returns>
        Task<WeekEvent> GetWeekEventByWeekAndTeamsAsync(string weekId, string awayTeamId, string homeTeamId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all <see cref="WeekEvent"/> objects for a specified <see cref="SeasonWeek"/>
        /// </summary>
        /// <param name="weekId"><see cref="SeasonWeek"/> ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see cref="SeasonWeek"/> <see cref="WeekEvent"/> objects</returns>
        Task<IEnumerable<WeekEvent>> GetWeekEventsAsync(string weekId, CancellationToken cancellationToken = default);
    }
}
