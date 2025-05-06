using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GainPowerWhenCardIsDrawnMixin : IDrawCardWhenOnFieldMixin, IValueMixin<int>
{
    public string MixinId => "Gain Power When Card Is Drawn";
    public void OnCardDrawn(GameContext context, ICard card, IPlayer drawingPlayer)
    {
        if (card.Owner != drawingPlayer)
            return;

        var powerMixin = card.GetOfType<CardPowerMixin>().FirstOrDefault();
        if (powerMixin == null)
            return;

        powerMixin.Value++;
    }

    public int Value { get; set; } = 1;
}