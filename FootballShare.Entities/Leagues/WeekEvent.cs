using Dapper.Contrib.Extensions;

using System;
using System.ComponentModel.DataAnnotations;

namespace FootballShare.Entities.Leagues
{
    /// <summary>
    /// Base <see cref="WeekEvent"/> class
    /// </summary>
    [Table("WeekEvents")]
    public class WeekEvent : EditableEntity
    {
        /// <summary>
        /// <see cref="WeekEvent"/> unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Away <see cref="Team"/> score
        /// </summary>
        [Display(Name = "Away Team Score", ShortName = "Away Score")]
        public int AwayScore { get; set; }
        /// <summary>
        /// Away <see cref="Team"/> ID
        /// </summary>
        public string AwayTeamId { get; set; }
        /// <summary>
        /// Home <see cref="Team"/> score
        /// </summary>
        [Display(Name = "Home Team Score", ShortName = "Home Score")]
        public int HomeScore { get; set; }
        /// <summary>
        /// Home <see cref="Team"/> ID
        /// </summary>
        public string HomeTeamId { get; set; }
        /// <summary>
        /// Overtime count
        /// </summary>
        public int Overtime { get; set; }
        /// <summary>
        /// <see cref="WeekEvent"/> is/was postponed
        /// </summary>
        public bool Postponed { get; set; }
        /// <summary>
        /// <see cref="SeasonWeek"/> ID
        /// </summary>
        public string SeasonWeekId { get; set; }
        /// <summary>
        /// Date/Time of <see cref="WeekEvent"/>
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:f}")]
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Away <see cref="Team"/> details
        /// </summary>
        public Team AwayTeam = null;
        /// <summary>
        /// Home <see cref="Team"/> details
        /// </summary>
        public Team HomeTeam = null;
        /// <summary>
        /// <see cref="SeasonWeek"/> details
        /// </summary>
        public SeasonWeek Week = null;

        public override string ToString()
        {
            if(this.AwayTeam != null && this.HomeTeam != null)
            {
                return $"{AwayTeam.Name} @ {HomeTeam.Name}";
            }
            else
            {
                return Id.ToString();
            }
        }
    }
}
