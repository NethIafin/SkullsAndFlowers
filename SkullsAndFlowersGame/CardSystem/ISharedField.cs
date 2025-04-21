namespace SkullsAndFlowersGame.CardSystem;

public interface ISharedField : IMixinContainer, ICardContainer
{
    string? CollectionId { get; }
    int Count { get; }
}