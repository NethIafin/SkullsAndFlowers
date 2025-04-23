using SkullsAndFlowersGame.CardSystem.Managers;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class StandardWorldEndRoundCleanupMixin : IEndRoundMixin
{
    public string MixinId => "Standard World End Round Cleanup";
    public bool OnEndRound(GameContext context)
    {
        // this one explicitly can know about GameHandlers helper class and it is fine
        GameHandlers.StandardWorldCleanup(context);
        
        return true;
    }
}