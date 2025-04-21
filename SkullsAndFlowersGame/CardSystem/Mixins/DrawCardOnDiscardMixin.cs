using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DrawCardOnDiscardMixin : IDiscardCardMixin
{
    public string MixinId => "Draw Card On Discard";
    public void OnDiscard(GameContext context, ICard discardedCard)
    {
        if (discardedCard.Owner != null) context.ScheduleDrawAction(discardedCard.Owner);
    }
}