namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IPlayCardTargetingPlayer : IMixin
{
    void OnPlayedTargetingPlayer(GameContext context, ICard playedCard, IPlayer playedPlayer, IPlayer targetPlayer);
}