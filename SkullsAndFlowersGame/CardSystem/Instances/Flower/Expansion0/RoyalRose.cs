using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class RoyalRose : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("royal rose")
            .SetCardPower(8)
            .RequiresTargetCardWithPowerInHand(4)
            .AddHandler<RequiresDiscardTargetCardOnPlayMixin>()
            .Build();
    }
}