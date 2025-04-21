namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledReturnToHandAction : IPlannedAction
{
    public required ICard Card { get; set; }
    public required ICardContainer Source { get; set; }
}