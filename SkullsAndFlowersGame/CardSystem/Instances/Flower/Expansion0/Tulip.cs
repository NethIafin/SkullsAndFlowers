using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Tulip : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("tulip")
            .SetCardPower(4)
            .Build();
    }
}