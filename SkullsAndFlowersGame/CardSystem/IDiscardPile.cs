namespace SkullsAndFlowersGame.CardSystem;

public interface IDiscardPile : IMixinContainer, ICardContainer
{
    string CollectionId { get; }
    int Count { get; }
}