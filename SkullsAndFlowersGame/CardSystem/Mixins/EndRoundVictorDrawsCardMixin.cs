using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class EndRoundVictorDrawsCardMixin : IEndRoundMixin
{
    public string MixinId => "End Round Victor Draws Card";
    public bool OnEndRound(GameContext context)
    {
        var lastRoundWinner = context.GetOfType<LastRoundWinnerMixin>().FirstOrDefault()?.Value ?? [];
        foreach (var winner in lastRoundWinner)
        {
            context.ScheduleDrawAction(context.Players[winner]);
        }

        return true;
    }
}