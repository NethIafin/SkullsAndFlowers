namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IDiscardCardMixin : IMixin
{
    void OnDiscard(GameContext context, ICard discardedCard);
}