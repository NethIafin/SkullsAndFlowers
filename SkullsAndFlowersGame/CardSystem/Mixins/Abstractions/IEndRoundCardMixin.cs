namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IEndRoundCardMixin : IMixin
{
    void OnRoundEnd(GameContext context, ICard card);
}