namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IPostEndTurnCardMixin : IMixin
{
    void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer);
}