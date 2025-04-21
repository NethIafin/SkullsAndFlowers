namespace SkullsAndFlowersGame.CardSystem;

public interface IDeck : IMixinContainer, ICardContainer
{
    string? CollectionId { get; }
    bool AddCardToTop(ICard card);
    bool AddCardToBottom(ICard card);
    int Count { get; }
    ICard? Draw();
    void Shuffle();
}