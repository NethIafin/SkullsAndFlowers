namespace SkullsAndFlowersGame.CardSystem.ScheduledActions;

public class ScheduleDrawAction : IPlannedAction
{
    public required IPlayer DrawingPlayer { get; set; }
}