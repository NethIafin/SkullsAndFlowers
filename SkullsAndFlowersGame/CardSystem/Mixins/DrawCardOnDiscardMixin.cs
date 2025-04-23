using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DrawCardOnDiscardMixin : IDiscardCardMixin
{
    public string MixinId => "Draw Card On Discard";
    public int NumberOfCards { get; set; } = 1;
    public void OnDiscard(GameContext context, ICard discardedCard)
    {
        if (discardedCard.Owner != null) context.ScheduleDrawAction(discardedCard.Owner);
    }
}

public static class DrawCardOnDiscardMixinExtension
{
    public static MixinContainerBuilder<ICard> DrawCardOnDiscard(this MixinContainerBuilder<ICard> builder, int amount)
    {
        return builder.AddMixin(new DrawCardOnDiscardMixin { NumberOfCards = amount });
    }
}