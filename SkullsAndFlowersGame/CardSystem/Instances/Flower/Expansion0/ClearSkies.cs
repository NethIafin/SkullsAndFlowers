using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class ClearSkies: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartGlobal("clear skies")
            .DrawCardOnPlay(1)
            .Build();
    }
}