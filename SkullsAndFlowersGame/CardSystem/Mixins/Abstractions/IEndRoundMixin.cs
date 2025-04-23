namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IEndRoundMixin : IMixin
{
    bool OnEndRound(GameContext context);
}