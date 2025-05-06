using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class ReturnToPlayOnRemoveMixin : IEndRoundCardMixin
{
    public string MixinId => "Return To Play On Remove";
    
    public void OnRoundEnd(GameContext context, ICard card)
    {
        var owner = card.Owner?.MatchPlayerId;
        
        if (owner == null)
            return;

        context.SchedulePlaceAction(card, card.Owner!);
    }
}