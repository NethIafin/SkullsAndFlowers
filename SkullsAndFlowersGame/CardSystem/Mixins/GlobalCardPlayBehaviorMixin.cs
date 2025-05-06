using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GlobalCardPlayBehaviorMixin : IPlayCardMixin
{
    public string MixinId => "Global Card Play Behavior";
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        foreach (var card in context.SharedField.Cards)
        {
            context.ScheduleDiscardAction(card, playedPlayer);
        }

        field.RemoveCard(playedCard);
        context.SharedField.AddCard(playedCard);
        playedCard.Container = context.SharedField;
    }
}