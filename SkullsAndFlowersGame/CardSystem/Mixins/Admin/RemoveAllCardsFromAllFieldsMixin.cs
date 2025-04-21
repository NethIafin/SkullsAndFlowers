using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins.Admin;

public class RemoveAllCardsFromAllFieldsMixin : IPlayCardMixin
{
    public string MixinId => "!!! Remove All Cards From All Fields";
    public void OnPlayed(GameContext context, ICard playedCard, IPlayField field, IPlayer playedPlayer)
    {
        foreach (var playField in context.PlayFields)
        {
            foreach (var card in playField.Cards)
            {
                context.ScheduleDiscardAction(card, playField, playedPlayer);
            }
        }
        foreach (var card in context.SharedField.Cards)
        {
            context.ScheduleDiscardAction(card, context.SharedField, playedPlayer);
        }
    }
}