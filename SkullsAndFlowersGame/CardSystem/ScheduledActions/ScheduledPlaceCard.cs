namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledPlaceCard : IPlannedAction
{
    public required ICard Card { get; set; }
    public required IPlayer Player { get; set; }
}