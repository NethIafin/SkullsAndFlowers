using SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet.Token;
using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.BaseSet;

public class BuddyBudTree: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("buddy bud tree")
            .SetCardPower(0)
            .AddHandler<SpawnTokenWhenPlayedMixin<Bud>>()
            .AddHandler<SpawnTokenWhenPlayedMixin<Bud>>()
            .Build();
    }
}