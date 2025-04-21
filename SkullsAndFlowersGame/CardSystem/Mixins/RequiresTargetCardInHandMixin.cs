using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RequiresTargetCardInHandMixin : ITargetCardMixin
{
    public string MixinId => "Requires Target Card In Hand";
    public ICard? TargetCard { get; set; }
    public ICardContainer? TargetCardContainer { get; set; }
    public TargetingFlags ValidTarget => TargetingFlags.Hand;
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
        
        TargetCard = targetCard;
        TargetCardContainer = targetCardSource;
        
        return true;
    }
}