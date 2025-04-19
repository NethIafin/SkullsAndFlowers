namespace SkullsAndFlowersGame.CardSystem.Mixins;

public interface IRemoveCardMixin : IMixin
{
    void OnRemoved(GameContext context, ICard removedCard);
}