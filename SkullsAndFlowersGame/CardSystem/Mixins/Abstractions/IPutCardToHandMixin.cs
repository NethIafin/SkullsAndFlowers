namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IPutCardToHandMixin : IMixin
{
    bool OnTryPutCardToHand(GameContext context, ICard card);
}