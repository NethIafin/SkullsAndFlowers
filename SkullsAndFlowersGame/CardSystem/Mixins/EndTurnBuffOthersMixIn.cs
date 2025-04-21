using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class EndTurnBuffOthersMixIn : IValueMixin<int>, IEndTurnCardMixin, ICardFieldAwareMixin
{
    public string MixinId => "buff other cards";
    public void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer)
    {
        if (Field == null)
            return;

        foreach (var cardPower in Field.Cards.Where(x=>x != card).SelectMany(x=>x.GetOfType<CardCurrentPowerMixin>()))
        {
            cardPower.Value += Value;
        }
    }

    public IPlayField? Field { get; set; }
    public int Value { get; set; }
}

public static class EndTurnBuffOthersMixInExtension
{
    public static MixinContainerBuilder<ICard> AddSelflessBuffAbility(this MixinContainerBuilder<ICard> builder, int power)
    {
        return builder.AddMixin(new EndTurnBuffOthersMixIn { Value = power });
    }
}