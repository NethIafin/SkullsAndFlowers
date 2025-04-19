namespace SkullsAndFlowersGame.CardSystem.Mixins;

public interface IEndTurnCardMixin
{
    void OnTurnEnd(GameContext context, ICard card);
}