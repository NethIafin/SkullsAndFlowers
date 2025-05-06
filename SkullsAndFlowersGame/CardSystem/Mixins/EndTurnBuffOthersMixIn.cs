using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class EndTurnBuffOthersMixIn : IValueMixin<int>, IPostEndTurnCardMixin
{
    public string MixinId => "buff other cards";
    public void OnTurnEnd(GameContext context, ICard card, IPlayer activePlayer)
    {
        if (card.Container == null || card.Container is not IPlayField)
            return;

        foreach (var cardPower in card.Container.Cards.Where(x=>x != card).SelectMany(x=>x.GetOfType<CardCurrentPowerMixin>()))
        {
            cardPower.Value += Value;
        }
    }
    public int Value { get; set; }
}

public static class EndTurnBuffOthersMixInExtension
{
    public static MixinContainerBuilder<ICard> AddSelflessBuffAbility(this MixinContainerBuilder<ICard> builder, int power)
    {
        return builder.AddMixin(new EndTurnBuffOthersMixIn { Value = power });
    }
}