using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class CardPowerMixin : IValueMixin<int>
{
    public string MixinId => "Card Power";
    public int Value { get; set; }
}

public static class CardPowerMixinExtension
{
    public static MixinContainerBuilder<ICard> SetCardPower(this MixinContainerBuilder<ICard> builder, int power)
    {
        return builder.AddHandler<CardCurrentPowerMixin>().AddMixin(new CardPowerMixin { Value = power });
    }
}