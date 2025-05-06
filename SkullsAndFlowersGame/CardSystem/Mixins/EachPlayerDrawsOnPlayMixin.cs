using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class EachPlayerDrawsOnPlayMixin : IPlayCardMixin
{
    public string MixinId => "Each Player Draws On Play";
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        foreach (var player in context.Players)
            context.ScheduleDrawAction(player);
    }
}