namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IGameStartMixin : IMixin
{
    void OnGameStart(GameContext context);
}