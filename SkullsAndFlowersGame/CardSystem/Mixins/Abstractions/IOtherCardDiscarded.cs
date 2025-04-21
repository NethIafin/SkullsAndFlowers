namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IOtherCardDiscarded : IMixin
{
    void OnOtherCardDiscarded(GameContext context, ICard discardedCard, IPlayer player);
}