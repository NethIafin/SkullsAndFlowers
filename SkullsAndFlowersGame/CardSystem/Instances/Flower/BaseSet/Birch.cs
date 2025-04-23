using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class Birch : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("birch")
            .SetCardPower(4)
            .AddHandler<RequiresTargetCardInPlayMixin>()
            .AddHandler<RequiresTargetCardInPlayMixin>()
            .DiscardTargetedCards(2)
            .Build();
    }
}