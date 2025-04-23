using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GoToDiscardPileWhenRemovedMixin : IDiscardCardMixin, IEndRoundCardMixin
{
    public string MixinId => "Go To Discard Pile When Removed";
    public void OnRoundEnd(GameContext context, ICard card)
    {
        context.ScheduleToDiscardPileAction(card);
    }

    public void OnDiscard(GameContext context, ICard discardedCard)
    {
        context.ScheduleToDiscardPileAction(discardedCard);
    }
}