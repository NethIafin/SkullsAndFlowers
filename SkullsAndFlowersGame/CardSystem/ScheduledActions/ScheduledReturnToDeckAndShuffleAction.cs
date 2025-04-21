namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledReturnToDeckAndShuffleAction : IPlannedAction
{
    public required ICard Card { get; set; }
    public required IPlayField Source { get; set; }
    public required IPlayer Player { get; set; }
}