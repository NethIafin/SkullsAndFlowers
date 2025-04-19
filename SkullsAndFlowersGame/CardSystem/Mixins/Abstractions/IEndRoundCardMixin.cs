namespace SkullsAndFlowersGame.CardSystem.Mixins;

public interface IEndRoundCardMixin
{
    void OnRoundEnd(GameContext context, ICard card);
}