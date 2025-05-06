using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RemoveRandomCardFromHandOnRemoveMixin : IEndRoundCardMixin
{
    public string MixinId => "Remove Random Card From Hand On Remove Mixin";
    public void OnRoundEnd(GameContext context, ICard card)
    {
        var cardOwnerId = card.Owner?.MatchPlayerId;

        if (cardOwnerId == null)
            return;

        var listOfCards = context.PlayerHands[cardOwnerId.Value].Cards.ToList();

        var countOfCards = listOfCards.Count;

        if (countOfCards == 0)
            return;

        var random = new Random().Next(countOfCards);

        var cardToRemove = listOfCards[random];
        
        context.ScheduleDiscardAction(cardToRemove, card.Owner!);
    }
}