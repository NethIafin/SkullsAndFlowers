using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Monsoon: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartGlobal("monsoon")
            .AddHandler<DiscardTopDeckOnStartTurnMixin>()
            .Build();
    }
}