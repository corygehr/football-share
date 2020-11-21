namespace FootballShare.Entities.Betting
{
    /// <summary>
    /// <see cref="Wager"/> result status
    /// </summary>
    public enum WagerResult
    {
        None = -1,
        Push = 0,
        Loss = 1,
        Win = 2,
        Refund = 3
    }
}
