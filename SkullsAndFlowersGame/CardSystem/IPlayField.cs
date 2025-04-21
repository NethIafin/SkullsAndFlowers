namespace SkullsAndFlowersGame.CardSystem;

public interface IPlayField : IMixinContainer, ICardContainer
{
    string? CollectionId { get; }
    bool AddCardAtPosition(int position, ICard card);
    int Count { get; }
}