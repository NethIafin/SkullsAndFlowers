using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RequiresTargetCardWithPowerInHandMixin : ITargetCardMixin
{
    public string MixinId => "Requires Target Card With Power In Hand";
    public ICard? TargetCard { get; set; }
    public ICardContainer? TargetCardContainer { get; set; }
    public TargetingFlags ValidTarget => TargetingFlags.Hand;
    public int MinimumPower { get; set; } = 4; // Default to 4 or more
    
    public bool SetTarget(GameContext context, ICardContainer targetCardSource, ICard targetCard, ICard thisCard)
    {
        if (targetCardSource is not IPlayerHand)
        {
            return false;
        }

        if (thisCard == targetCard)
        {
            return false;
        }
        
        // Check if the card has enough power
        var powerMixin = targetCard.GetOfType<CardPowerMixin>().FirstOrDefault();
        if (powerMixin == null || powerMixin.Value < MinimumPower)
        {
            return false;
        }
        
        TargetCard = targetCard;
        TargetCardContainer = targetCardSource;
        
        return true;
    }
}

public static class RequiresTargetCardWithPowerInHandMixinExtension
{
    public static MixinContainerBuilder<ICard> RequiresTargetCardWithPowerInHand(
        this MixinContainerBuilder<ICard> builder, int minimumPower = 4)
    {
        return builder.AddMixin(new RequiresTargetCardWithPowerInHandMixin { MinimumPower = minimumPower });
    }
}