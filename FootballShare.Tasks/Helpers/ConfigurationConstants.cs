namespace FootballShare.Tasks.Helpers
{
    /// <summary>
    /// Configuration value names
    /// </summary>
    public static class ConfigurationConstants
    {
        /// <summary>
        /// Amount of time to wait between refresh cycles, in hours
        /// </summary>
        public const string RefreshIntervalHours = "RefreshIntervalHours";
        /// <summary>
        /// Semicolon (;) delimited string of leagues which should have their data refreshed
        /// </summary>
        public const string TargetLeagues = "TargetLeagues";
    }
}
