namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface IValueMixin<T> : IMixin
{
    T Value { get; set; }
}