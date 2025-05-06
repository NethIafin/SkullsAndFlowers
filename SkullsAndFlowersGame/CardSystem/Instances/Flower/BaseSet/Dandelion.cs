using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class Dandelion : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("dandelion")
            .SetCardPower(1)
            .GivePermanentPowerToTargetCard()
            .Build();
    }
}