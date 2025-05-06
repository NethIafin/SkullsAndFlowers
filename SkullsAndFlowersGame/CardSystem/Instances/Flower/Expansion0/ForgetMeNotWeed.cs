using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class ForgetMeNotWeed: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("forget-me-not weed")
            .SetCardPower(1)
            .AddHandler<ReturnToPlayOnRemoveMixin>()
            .AddHandler<ReturnToPlayOnDiscardMixin>()
            .Build();
    }
}