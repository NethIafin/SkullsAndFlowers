namespace SkullsAndFlowersGame.CardSystem;

[Flags]
public enum TargetingFlags
{
    None = 0,
    Field = 1,
    Hand = 2,
    Deck = 4,
    Discard = 8,
    CommonField = 16
}