using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class Spruce: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("spruce")
            .SetCardPower(1)
            .DrawCardOnPlay(1)
            .AddHandler<RemoveRandomCardFromHandOnRemoveMixin>()
            .Build();
    }
}