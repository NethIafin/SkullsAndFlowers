namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

// use this as rarely as possible
public interface IPlaceCardMixin : IMixin
{
    bool OnTryCardPlaced(GameContext context, ICardContainer container, ICard card);
}