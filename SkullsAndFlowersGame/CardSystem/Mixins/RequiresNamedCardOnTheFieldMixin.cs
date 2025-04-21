using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RequiresNamedCardOnTheFieldMixin : ITargetCardMixin
{
    public string MixinId => "Requires Named Card On The Field";
    public ICard? TargetCard { get; set; }
    public ICardContainer? TargetCardContainer { get; set; }
    public TargetingFlags ValidTarget => TargetingFlags.Field | TargetingFlags.CommonField;
    
    public required IEnumerable<string> CardTypeNames { get; init; }
    
    public bool SetTarget(GameContext context, ICardContainer targetCardSource, ICard targetCard, ICard thisCard)
    {
        if (targetCardSource is not IPlayField && targetCardSource is not ISharedField)
        {
            return false;
        }

        if (CardTypeNames.Contains(targetCard.Identifier))
        {
            TargetCard = targetCard;
            TargetCardContainer = targetCardSource;
            return true;
        }

        return false;
    }
}

public static class RequiresNamedCardOnTheFieldMixinExtension
{
    public static MixinContainerBuilder<ICard> RequiresNamedTargetOnField(this MixinContainerBuilder<ICard> builder, params string[] targets)
    {
        return builder.AddMixin(new RequiresNamedCardOnTheFieldMixin { CardTypeNames = targets });
    }
}