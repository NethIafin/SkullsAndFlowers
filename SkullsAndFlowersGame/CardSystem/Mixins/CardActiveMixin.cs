using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class CardActiveMixin : IValueMixin<bool>
{
    public string MixinId { get; }
    public bool Value { get; set; } = true; 
}