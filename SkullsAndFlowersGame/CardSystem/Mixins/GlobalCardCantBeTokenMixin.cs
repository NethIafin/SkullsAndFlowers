using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GlobalCardCantBeTokenMixin : IPlaceCardMixin
{
    public string MixinId => "Global Card Cant Be Token";
    public bool OnTryCardPlaced(GameContext context, ICardContainer container, ICard card)
    {
        if (card.GetOfType<IsTokenMixin>().Any())
            return false;
        return true;
    }
}