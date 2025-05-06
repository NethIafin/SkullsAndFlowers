using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DestroyRandomDiscardGiveOtherCardPowerMixin : IEndTurnCardMixin
{
    public string MixinId => "Destroy Random Discard Give Other Card Power";
    public void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer)
    {
        var cardOwnerId = card.Owner?.MatchPlayerId;

        if (cardOwnerId == null)
            return;
        
        if (card.Owner != activePlayer)
            return;

        var listOfCards = context.DiscardPiles[cardOwnerId.Value].Cards.ToList();

        var countOfCards = listOfCards.Count;

        if (countOfCards == 0)
            return;

        var random = new Random().Next(countOfCards);

        var cardToDestroy = listOfCards[random];
        
        var listOfFieldCards = context.PlayFields[cardOwnerId.Value].Cards.Where(x => x != card).ToList();
        
        var countOfFieldCards = listOfFieldCards.Count;

        if (countOfFieldCards == 0)
            return;
        
        random = new Random().Next(countOfFieldCards);

        var cardToBuff = listOfFieldCards[random];

        context.DiscardPiles[cardOwnerId.Value].RemoveCard(cardToDestroy);

        var mixin = cardToBuff.GetOfType<CardPowerMixin>().FirstOrDefault();
        if (mixin != null) mixin.Value += 1;
    }
}