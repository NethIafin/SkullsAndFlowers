namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IPlayCardTargetingCard : IMixin
{
    void OnPlayedTargetingCard(GameContext context, ICard playedCard, IPlayer playedPlayer, ICard targetCard);
}