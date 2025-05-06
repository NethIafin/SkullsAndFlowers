using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class SetAllCardsOnFieldPowerToOneMixin : IPostEndTurnCardMixin
{
    public string MixinId => "Set All Cards On Field Power To One";
    public void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer)
    {
        foreach (var field in context.PlayFields)
        {
            foreach (var playCard in field.Cards)
            {
                var power = playCard.GetOfType<CardCurrentPowerMixin>().FirstOrDefault();
                if (power != null) power.Value = 1;
            }
        }
    }
}