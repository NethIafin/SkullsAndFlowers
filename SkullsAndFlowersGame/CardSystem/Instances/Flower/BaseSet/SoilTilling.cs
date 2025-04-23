using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class SoilTilling : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartGlobal("soil tilling")
            .DrawCardOnRemoval(3)
            .Build();
    }
}