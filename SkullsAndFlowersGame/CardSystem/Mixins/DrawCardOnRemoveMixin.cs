using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DrawCardOnRemoveMixin: IEndRoundCardMixin
{
    public string MixinId => "Draw Card On Removal";
    public void OnRoundEnd(GameContext context, ICard card)
    {
        if (card.Owner != null)
        {
            for (var i = 0; i < NumberOfCards; i++)
                context.ScheduleDrawAction(card.Owner);
        }
    }
    public int NumberOfCards { get; set; } = 1;
}

public static class DrawCardOnRemoveMixinExtension
{
    public static MixinContainerBuilder<ICard> DrawCardOnRemoval(this MixinContainerBuilder<ICard> builder, int amount)
    {
        return builder.AddMixin(new DrawCardOnRemoveMixin { NumberOfCards = amount });
    }
}