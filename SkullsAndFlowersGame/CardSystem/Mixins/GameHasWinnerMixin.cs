using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class GameHasWinnerMixin : IEndRoundMixin, IValueMixin<int?>
{
    public string MixinId => "Game Has Winner";

    public int TargetWinCount { get; set; } = 2;
    
    public bool OnEndRound(GameContext context)
    {
        Value = null;
        for (var i = 0; i < context.Players.Count; i++)
        {
            if (context.Players[i].Score >= TargetWinCount)
            {
                Value = i;
                return false;
            }
        }

        return true;
    }

    public int? Value { get; set; }
}