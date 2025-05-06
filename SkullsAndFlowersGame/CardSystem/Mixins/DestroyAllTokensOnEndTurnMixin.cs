using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DiscardAllTokensOnEndTurnMixin : IEndTurnCardMixin
{
    public string MixinId => "Discard All Tokens On End Turn";
    public void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer)
    {
        if(card.Owner == null)
            return;
        
        foreach (var field in context.PlayFields)
        {
            foreach (var fieldCard in field.Cards)
            {
                var isToken = fieldCard.GetOfType<IsTokenMixin>().Any();
                if(isToken)
                    context.ScheduleDiscardAction(fieldCard, card.Owner);
            }
        }
    }
}