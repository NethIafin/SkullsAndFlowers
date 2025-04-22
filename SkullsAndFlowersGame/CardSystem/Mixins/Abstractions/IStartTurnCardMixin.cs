namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IStartTurnCardMixin : IMixin
{
    void OnTurnStart(GameContext context, ICard card, IPlayer activePlayer);
}