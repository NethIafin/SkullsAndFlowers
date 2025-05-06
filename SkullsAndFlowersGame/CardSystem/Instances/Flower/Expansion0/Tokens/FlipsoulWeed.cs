using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0.Tokens;

public class FlipsoulWeed : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartToken("flipsoul weed")
            .SetCardPower(2)
            .Build();
    }
}