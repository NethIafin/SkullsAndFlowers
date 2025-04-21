namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledDiscardAction : IPlannedAction
{
    public required ICard Card { get; set; }
    public required ICardContainer Source { get; set; }
    public required IPlayer Player { get; set; }
}