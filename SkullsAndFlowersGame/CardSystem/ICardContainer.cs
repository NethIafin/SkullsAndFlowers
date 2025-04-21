namespace SkullsAndFlowersGame.CardSystem;

public interface ICardContainer
{
    IEnumerable<ICard> Cards { get; }
    bool AddCard(ICard card);
    bool RemoveCard(ICard card);
}