using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RequiresDiscardTargetCardOnPlayMixin : IPlayCardMixin
{
    public string MixinId => "Discard Target Card On Play";
    public int RequiredCount { get; set; } = 1;
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        var foundTargets = 0;
        foreach (var target in playedCard.GetOfType<ITargetCardMixin>())
        {
            if (target.TargetCard == null || target.TargetCardContainer == null)
                continue;

            foundTargets++;
        }

        if (RequiredCount < foundTargets)
        {
            var deactivate = playedCard.GetOfType<CardActiveMixin>().FirstOrDefault();
            if (deactivate != null) deactivate.Value = false;
            return;
        }
        
        foreach (var target in playedCard.GetOfType<ITargetCardMixin>())
        {
            if (target.TargetCard == null || target.TargetCardContainer == null)
                continue;

            context.ScheduleDiscardAction(target.TargetCard, target.TargetCardContainer, playedPlayer);
        }
    }
}