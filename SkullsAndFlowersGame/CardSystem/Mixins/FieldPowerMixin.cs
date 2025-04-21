using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class FieldPowerMixin : IValueMixin<int>, IEndTurnFieldMixin
{
    public string MixinId => "Field Power";
    public int Value { get; set; }
    
    public void OnTurnEnd(GameContext context, IPlayField field, IPlayer activePlayer)
    {
        var totalPower = field.Cards
            .SelectMany(card => card.GetOfType<CardCurrentPowerMixin>())
            .Sum(value => value.Value);

        Value = totalPower;
    }
}