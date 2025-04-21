namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IOtherCardPlayed : IMixin
{
    void OnOtherCardPlayed(GameContext context, ICard playedCard, IPlayer playedPlayer);
}