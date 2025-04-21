namespace SkullsAndFlowersGame.CardSystem;

public class Player : IPlayer
{
    public bool Passed { get; set; }
    public int Score { get; set; }
    public int MatchPlayerId { get; set; }
}