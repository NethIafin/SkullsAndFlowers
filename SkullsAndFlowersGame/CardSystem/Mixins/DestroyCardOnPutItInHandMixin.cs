using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DestroyCardOnPutItInHandMixin : IPutCardToHandMixin
{
    public string MixinId => "Discard Card On Put It In Hand";
    public bool OnTryPutCardToHand(GameContext context, ICard card)
    {
        return false;
    }
}