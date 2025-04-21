namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IPlayCardMixin : IMixin
{
    void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer);
}