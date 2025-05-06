namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IDrawCardWhenOnFieldMixin : IMixin
{
    void OnCardDrawn(GameContext context, ICard card, IPlayer drawingPlayer);
}