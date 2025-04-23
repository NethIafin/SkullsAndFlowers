using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class LastRoundWinnerMixin : IValueMixin<int[]>, IEndRoundMixin
{
    public string MixinId => "Last Round Winner";
    
    public bool OnEndRound(GameContext context)
    {
        var result = new List<int>();
        var boardPowers = new int?[context.PlayFields.Count];
        var maxPower = 0;
        for (var i = 0; i < context.PlayFields.Count; i++)
        {
            boardPowers[i] = context.PlayFields[i].GetOfType<FieldPowerMixin>().Max(x => x.Value);
            if (boardPowers[i] != null && boardPowers[i] > maxPower)
                maxPower = boardPowers[i]!.Value;
        }

        for (var i = 0; i < boardPowers.Length; i++)
        {
            if (boardPowers[i] == maxPower)
            {
                context.Players[i].Score++;
                result.Add(i);
            }
        }

        Value = result.ToArray();

        return true;
    }

    public int[] Value { get; set; } = [];
}