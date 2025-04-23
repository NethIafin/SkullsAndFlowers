using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GivePermanentPowerToTargetCardMixin : IPlayCardMixin
{
    public string MixinId => "Give Permanent Power To Target Card";
    public int RequiredTargetCount { get; set; } = 1;
    public int BuffAmount { get; set; } = 1;
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
        
        if (foundTargets != RequiredTargetCount)
        {
            var deactivate = playedCard.GetOfType<CardActiveMixin>().FirstOrDefault();
            if (deactivate != null) deactivate.Value = false;
            return;
        }
        
        foreach (var validTarget in validTargets)
        {
            if (validTarget is not { TargetCard: not null, TargetCardContainer: not null }) continue;

            foreach (var cardPowerMixin in validTarget.TargetCard.GetOfType<CardPowerMixin>())
            {
                cardPowerMixin.Value += BuffAmount;
            }
        }
    }
}

public static class GivePermanentPowerToTargetCardMixinExtension
{
    public static MixinContainerBuilder<ICard> GivePermanentPowerToTargetCard(this MixinContainerBuilder<ICard> builder,
        int buffAmount = 1, int cardCountRequirement = 1)

    {
        return builder.AddMixin(new GivePermanentPowerToTargetCardMixin
            { RequiredTargetCount = cardCountRequirement, BuffAmount = buffAmount });
    }
}