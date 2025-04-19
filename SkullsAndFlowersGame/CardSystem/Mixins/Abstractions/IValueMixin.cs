namespace SkullsAndFlowersGame.CardSystem.Mixins;

public interface IValueMixin<T> : IMixin
{
    T Value { get; set; }
}