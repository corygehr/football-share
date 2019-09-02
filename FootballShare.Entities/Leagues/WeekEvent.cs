﻿using Dapper.Contrib.Extensions;

using System;

namespace FootballShare.Entities.League
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
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Away <see cref="Team"/> score
        /// </summary>
        public int AwayScore { get; set; }
        /// <summary>
        /// Away <see cref="Team"/> ID
        /// </summary>
        public string AwayTeamId { get; set; }
        /// <summary>
        /// Home <see cref="Team"/> score
        /// </summary>
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
        public DateTime Time { get; set; }

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
    }
}