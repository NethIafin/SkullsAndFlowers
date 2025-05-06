using SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0.Tokens;
using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class RoyalTulip : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("royal tulip")
            .SetCardPower(4)
            .RequireReturnToHandOneNamedTargetOnField("tulip")
            .Build();
    }
}