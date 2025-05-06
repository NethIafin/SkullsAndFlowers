using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Sorrel: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("sorrel")
            .SetCardPower(0)
            .AddHandler<GainPowerWhenCardIsDrawnMixin>()
            .Build();
    }
}