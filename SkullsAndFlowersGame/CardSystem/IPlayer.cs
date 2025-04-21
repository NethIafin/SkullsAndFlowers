namespace SkullsAndFlowersGame.CardSystem;

public interface IPlayer
{
    bool Passed { get; set; }
    int Score { get; set; }
    int MatchPlayerId { get; set; }
}