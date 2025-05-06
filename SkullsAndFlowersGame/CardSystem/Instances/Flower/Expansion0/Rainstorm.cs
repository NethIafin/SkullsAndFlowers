using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Rainstorm: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartGlobal("rainstorm")
            .AddHandler<SetAllCardsOnFieldPowerToOneMixin>()
            .Build();
    }
}