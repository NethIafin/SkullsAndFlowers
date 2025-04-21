namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IEndTurnCardMixin : IMixin
{
    void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer);
}