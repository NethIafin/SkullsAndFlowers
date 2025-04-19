namespace SkullsAndFlowersGame.CardSystem.Mixins;

public interface IPlayCardMixin : IMixin
{
    void OnPlayed(GameContext context, ICard playedCard, IPlayer playedPlayer);
}