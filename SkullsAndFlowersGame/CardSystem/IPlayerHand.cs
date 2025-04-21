namespace SkullsAndFlowersGame.CardSystem;

public interface IPlayerHand : IMixinContainer, ICardContainer
{
    string? CollectionId { get; }
    int Count { get; }
}