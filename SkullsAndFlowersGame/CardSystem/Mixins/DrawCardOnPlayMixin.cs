using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DrawCardOnPlayMixin : IPlayCardMixin
{
    public string MixinId => "Draw Card On Play Mixin";
    public int NumberOfCards { get; set; } = 1;
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        if (playedCard.Owner != null)
        {
            for (var i = 0; i < NumberOfCards; i++)
                context.ScheduleDrawAction(playedCard.Owner);
        }
    }
}

public static class DrawCardOnPlayMixinExtension
{
    public static MixinContainerBuilder<ICard> DrawCardOnPlay(this MixinContainerBuilder<ICard> builder, int amount)
    {
        return builder.AddMixin(new DrawCardOnPlayMixin { NumberOfCards = amount });
    }
}