using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class ScorchingSunbeam: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartGlobal("scorching sunbeam")
            .AddHandler<DiscardAllTokensOnEndTurnMixin>()
            .Build();
    }
}