using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class StripedBirch: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("striped birch")
            .SetCardPower(3)
            .AddHandler<EachPlayerDrawsOnPlayMixin>()
            .Build();
    }
}