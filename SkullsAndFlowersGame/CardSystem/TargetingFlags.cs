namespace SkullsAndFlowersGame.CardSystem;

[Flags]
public enum TargetingFlags
{
    None = 0,
    Field = 2<<0,
    Hand = 2<<1,
    Deck = 2<<2,
    Discard = 2<<3,
    CommonField = 2<<4,
    Player = 2<<5,
    Own = 2<<16,
    Enemy = 2<<17,
    Any = 2<<18
}