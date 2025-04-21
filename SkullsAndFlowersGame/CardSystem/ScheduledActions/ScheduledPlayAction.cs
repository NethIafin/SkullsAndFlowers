namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduledPlayAction : IPlannedAction
{
    public required ICard Card { get; set; }
    public required IPlayField Destination { get; set; }
    public required IPlayer Player { get; set; }
}