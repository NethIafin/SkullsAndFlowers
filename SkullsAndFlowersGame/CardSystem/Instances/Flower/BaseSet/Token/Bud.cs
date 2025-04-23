using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet.Token;

public class Bud: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartToken("bud")
            .SetCardPower(1)
            .Build();
    }
}