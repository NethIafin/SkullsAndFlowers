using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DiscardRandomCardFromHandOnStartTurnThenDrawCardMixin : IStartTurnCardMixin
{
    public string MixinId => "Discard Random Card From Hand On Start Turn Then Draw Card";
    public void OnTurnStart(GameContext context, ICard card, IPlayer activePlayer)
    {
        var cardOwner = card.Owner;
        if (cardOwner != activePlayer)
            return;
        
        var listOfCards = context.PlayerHands[cardOwner.MatchPlayerId].Cards.ToList();

        var countOfCards = listOfCards.Count;

        if (countOfCards == 0)
            return;

        var random = new Random().Next(countOfCards);

        var cardToRemove = listOfCards[random];
        
        context.ScheduleDiscardAction(cardToRemove, cardOwner);
        context.ScheduleDrawAction(cardOwner);
    }
}