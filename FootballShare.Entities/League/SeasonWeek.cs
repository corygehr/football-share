using Dapper.Contrib.Extensions;
using System;

namespace FootballShare.Entities.League
{
    /// <summary>
    /// Base <see cref="SeasonWeek"/> class
    /// </summary>
    [Table("SeasonWeeks")]
    public class SeasonWeek
    {
        /// <summary>
        /// <see cref="SeasonWeek"/> unique identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> is a Championship round
        /// </summary>
        public bool IsChampionship { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> is a Playoff round
        /// </summary>
        public bool IsPlayoff { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> is a Preseason round
        /// </summary>
        public bool IsPreseason { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> end date
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// <see cref="Season"/> of this <see cref="SeasonWeek"/>
        /// </summary>
        public string SeasonId { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> order in <see cref="Season"/>
        /// </summary>
        public int Sequence { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// <see cref="Season"/> details
        /// </summary>
        public Season Season = null;
    }
}
