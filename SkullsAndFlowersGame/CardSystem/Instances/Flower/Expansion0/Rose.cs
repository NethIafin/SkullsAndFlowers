using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Rose : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("rose")
            .SetCardPower(6)
            .AddHandler<RequiresTargetCardInHandMixin>()
            .AddHandler<RequiresDiscardTargetCardOnPlayMixin>()
            .AddHandler<DrawCardOnDiscardMixin>()
            .Build();
    }
}