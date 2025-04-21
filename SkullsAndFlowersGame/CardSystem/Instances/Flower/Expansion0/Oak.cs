using SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0.Tokens;
using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Oak : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("oak")
            .SetCardPower(0)
            .AddHandler<SpawnTokenWhenPlayedMixin<Acorn>>()
            .AddHandler<SpawnTokenWhenPlayedMixin<Acorn>>()
            .AddHandler<SpawnTokenWhenPlayedMixin<Acorn>>()
            .Build();
    }
}