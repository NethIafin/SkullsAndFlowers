namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IEndTurnFieldMixin : IMixin
{
    void OnTurnEnd(GameContext context, IPlayField field, IPlayer activePlayer);
}