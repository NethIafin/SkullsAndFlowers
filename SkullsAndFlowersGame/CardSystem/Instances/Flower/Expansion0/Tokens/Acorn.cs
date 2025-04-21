using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0.Tokens;

public class Acorn : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartToken("acorn")
            .SetCardPower(1)
            .Build();
    }
}