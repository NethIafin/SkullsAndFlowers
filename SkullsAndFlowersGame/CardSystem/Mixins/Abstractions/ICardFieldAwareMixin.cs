namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface ICardFieldAwareMixin : IMixin
{
    IPlayField? Field { get; set; }
}