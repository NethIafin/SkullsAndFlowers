using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class SoilFertilizing : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartGlobal("soil fertilizing")
            .DrawCardOnRemoval(3)
            .Build();
    }
}