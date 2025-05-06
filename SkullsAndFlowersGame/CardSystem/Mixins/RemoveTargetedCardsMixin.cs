using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DiscardTargetedCardsMixin : IPlayCardMixin
{
    public string MixinId => "Discard Targeted Cards";
    public int RequireNumberOfTargetCards { get; set; } = 1;
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        var validTargets = playedCard.GetOfType<ITargetCardMixin>()
            .Where(x => x is { TargetCard: not null, TargetCardContainer: not null });

        var foundTargets = 0;
        
        foreach (var validTarget in validTargets)
        {
            if (validTarget is not { TargetCard: not null, TargetCardContainer: not null }) continue;

            foundTargets++;
        }
        
        if (foundTargets != RequireNumberOfTargetCards)
        {
            var deactivate = playedCard.GetOfType<CardActiveMixin>().FirstOrDefault();
            if (deactivate != null) deactivate.Value = false;
            return;
        }
        
        foreach (var validTarget in validTargets)
        {
            if (validTarget is not { TargetCard: not null, TargetCardContainer: not null }) continue;
            
            context.ScheduleDiscardAction(validTarget.TargetCard, playedPlayer);
        }
    }
}

public static class DiscardTargetedCardsMixinExtension
{
    public static MixinContainerBuilder<ICard> DiscardTargetedCards(this MixinContainerBuilder<ICard> builder, int countOfCards = 1)
    {
        return builder.AddMixin(new DiscardTargetedCardsMixin { RequireNumberOfTargetCards = countOfCards });
    }
}