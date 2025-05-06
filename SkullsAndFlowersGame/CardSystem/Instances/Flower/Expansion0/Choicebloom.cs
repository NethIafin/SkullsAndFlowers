using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Choicebloom : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("choicebloom")
            .SetCardPower(1)
            .DrawCardOnPlay(1)
            .Build();
    }
}