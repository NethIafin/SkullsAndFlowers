using SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0.Tokens;
using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class SoulflipWeed: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("soulflip weed")
            .SetCardPower(6)
            .AddHandler<SpawnTokenForAllEnemiesOnDiscardOrRemoveMixin<FlipsoulWeed>>()
            .Build();
    }
}