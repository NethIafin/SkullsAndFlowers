using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class RequiresTargetCardInPlayMixin: ITargetCardMixin
{
    public string MixinId => "Requires Target Card In Play";
    public ICard? TargetCard { get; set; }
    public ICardContainer? TargetCardContainer { get; set; }
    public TargetingFlags ValidTarget => TargetingFlags.Own | TargetingFlags.Field;
    public bool SetTarget(GameContext context, ICardContainer targetCardSource, ICard targetCard, ICard thisCard)
    {
        if (targetCardSource is not IPlayField && targetCardSource is not ISharedField)
        {
            return false;
        }

        if (targetCard.Owner != thisCard.Owner)
        {
            return false;
        }

        TargetCard = targetCard;
        TargetCardContainer = targetCardSource;
        
        return true;
    }
}