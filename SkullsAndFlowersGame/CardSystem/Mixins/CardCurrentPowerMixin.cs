using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class CardCurrentPowerMixin : IValueMixin<int?>, IPostEndTurnCardMixin
{
    public string MixinId => "Card Power";
    public int? Value { get; set; }
    
    public void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer)
    {
        var totalPower = card.GetOfType<CardPowerMixin>().Sum(x => x.Value);
        Value = totalPower;
    }
}