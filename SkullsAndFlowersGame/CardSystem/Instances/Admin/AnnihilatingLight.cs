using SkullsAndFlowersGame.CardSystem.Mixins.Admin;

namespace SkullsAndFlowersGame.CardSystem.Instances.Admin;

public class AnnihilatingLight: ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("!!! annihilating light")
            .AddHandler<RemoveAllCardsFromAllFieldsMixin>()
            .Build();
    }
}