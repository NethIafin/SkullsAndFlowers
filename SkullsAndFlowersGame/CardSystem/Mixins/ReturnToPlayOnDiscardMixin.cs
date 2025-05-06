using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class ReturnToPlayOnDiscardMixin : IDiscardCardMixin
{
    public string MixinId => "Return To Play On Discard";
    public void OnDiscard(GameContext context, ICard discardedCard)
    {
        var owner = discardedCard.Owner?.MatchPlayerId;
        
        if (owner == null)
            return;
        
        context.SchedulePlaceAction(discardedCard, discardedCard.Owner!);
    }
}