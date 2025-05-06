using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class Nutrient: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("nutrient")
            .SetCardPower(0)
            .AddHandler<DestroyRandomDiscardGiveOtherCardPowerMixin>()
            .Build();
    }
}