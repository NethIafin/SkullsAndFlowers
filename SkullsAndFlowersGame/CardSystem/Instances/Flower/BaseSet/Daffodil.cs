using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class Daffodil : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("daffodil")
            .SetCardPower(1)
            .GivePermanentPowerToTargetCard()
            .Build();
    }
}