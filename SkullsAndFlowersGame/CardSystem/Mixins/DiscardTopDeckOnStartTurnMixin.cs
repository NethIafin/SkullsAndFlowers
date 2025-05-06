using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class DiscardTopDeckOnStartTurnMixin : IStartTurnCardMixin
{
    public string MixinId => "Discard Top Deck On Start Turn";
    public void OnTurnStart(GameContext context, ICard card, IPlayer activePlayer)
    {
        if (card.Owner != activePlayer)
            return;
        
        var activePlayerId = activePlayer.MatchPlayerId;

        var drawnCard = context.Decks[activePlayerId].Draw();
        
        if(drawnCard !=null)
            context.ScheduleDiscardAction(drawnCard, activePlayer);
    }
}