namespace SkullsAndFlowersGame.CardSystem;

public interface ICard : IMixinContainer
{
    string? Identifier { get; }
    IPlayer? Owner { get; set; }
}