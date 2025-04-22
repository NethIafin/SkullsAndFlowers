using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DefaultGameStartMixin : IGameStartMixin
{
    public string MixinId => "Default Game Start";
    public int StartingCardsToDraw { get; set; } = 8;
    public void OnGameStart(GameContext context)
    {
        foreach (var player in context.Players)
        {
            for(var i=0; i<StartingCardsToDraw; i++)
                context.ScheduleDrawAction(player);
        }
    }
}