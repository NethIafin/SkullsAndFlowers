using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RequireReturnToHandOneNamedTargetOnFieldMixin : IPlayCardMixin
{
    public string MixinId => "Require Return To Hand One Named Target On Field";
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        var validTargets = playedCard.GetOfType<RequiresNamedCardOnTheFieldMixin>()
            .Where(x => x is { TargetCard: not null, TargetCardContainer: not null });

        var foundTarget = false;
        
        foreach (var validTarget in validTargets)
        {
            if (validTarget is not { TargetCard: not null, TargetCardContainer: not null }) continue;
            
            context.ScheduleReturnToHandAction(validTarget.TargetCard, validTarget.TargetCardContainer);

            foundTarget = true;
        }

        if (!foundTarget)
        {
            var deactivate = playedCard.GetOfType<CardActiveMixin>().FirstOrDefault();
            if (deactivate != null) deactivate.Value = false;
        }
    }
}

public static class RequireReturnToHandOneNamedTargetOnFieldMixinExtension
{
    public static MixinContainerBuilder<ICard> RequireReturnToHandOneNamedTargetOnField(this MixinContainerBuilder<ICard> builder, params string[] targets)
    {
        return builder.AddMixin(new RequiresNamedCardOnTheFieldMixin { CardTypeNames = targets })
                .AddHandler<RequireReturnToHandOneNamedTargetOnFieldMixin>();
    }
}