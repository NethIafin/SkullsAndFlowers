using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GoToDiscardPileWhenRemovedMixin : IDiscardCardMixin
{
    public string MixinId => "Go To Discard Pile When Removed";
    public void OnDiscard(GameContext context, ICard discardedCard)
    {
        context.ScheduleToDiscardPileAction(discardedCard);
    }
}