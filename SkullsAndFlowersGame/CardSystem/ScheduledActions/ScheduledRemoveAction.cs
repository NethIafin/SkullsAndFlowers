namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledRemoveAction : IPlannedAction
{
    public required ICard Card { get; set; }
    public required IPlayField Source { get; set; }
}