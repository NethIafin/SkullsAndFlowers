using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class SpawnTokenForAllEnemiesOnDiscardOrRemoveMixin<T> : IDiscardCardMixin, IEndRoundCardMixin where T : ICardTemplate, new()
{
    public string MixinId => "SpawnTokenForAllEnemiesOnDiscardOrRemove";
    public void OnRoundEnd(GameContext context, ICard card)
    {
        InternalAction(context, card);
    }

    public void OnDiscard(GameContext context, ICard discardedCard)
    {
        InternalAction(context, discardedCard);
    }

    private void InternalAction(GameContext context, ICard card)
    {
        if (card.Owner == null)
            return;

        foreach (var player in context.Players)
        {
            if(player == card.Owner)
                continue;

            var templateCard = new T();

            var token = templateCard.GenerateCard();
            token.Owner = player;
            
            context.SchedulePlaceAction(token, player);
        }
    }
}