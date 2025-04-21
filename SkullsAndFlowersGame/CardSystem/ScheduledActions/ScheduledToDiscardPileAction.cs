namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledToDiscardPileAction : IPlannedAction
{
    public required ICard Card { get; set; }
}