using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Mutators.WorldStart;

public class DefaultWorld : IWorldTemplate
{
    public GameContext PrepareWorld()
    {
        return MixinContainerBuilder.StartWorld()
            .AddHandler<DefaultGameStartMixin>()
            .Build();
    }
}