namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IRemoveCardMixin : IMixin
{
    void OnRemoved(GameContext context, ICard removedCard);
}