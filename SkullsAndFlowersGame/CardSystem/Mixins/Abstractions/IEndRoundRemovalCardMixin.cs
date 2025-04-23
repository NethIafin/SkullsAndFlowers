namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IEndRoundRemovalCardMixin : IMixin
{
    void OnRoundEndRemoval(GameContext context, ICard card);
}