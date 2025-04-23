using System.Diagnostics;
using SkullsAndFlowersGame.CardSystem.ScheduledActions;

namespace SkullsAndFlowersGame.CardSystem.Managers;

public class TurnManager
{
    public GameContext Context { get; set; }
    
    public bool EndTurn()
    {
        GameHandlers.EndTurn(Context, Context.Players[Context.ActivePlayer]);
        GameHandlers.DequeueAllActions(Context);
        Context.ActivePlayer++;
        if (Context.ActivePlayer >= Context.Players.Count)
        {
            Context.ActivePlayer = 0;
            Context.Turn++;
            if (Context.Players.All(x => x.Passed))
                return false;
        }

        return true;
    }

    public bool StartTurn()
    {
        if (Context.Players[Context.ActivePlayer].Passed)
            return false;
        
        GameHandlers.StartTurn(Context, Context.Players[Context.ActivePlayer]);
        GameHandlers.DequeueAllActions(Context);

        return true;
    }

    public bool StartGame()
    {
        if (Context.Players.Count < 2)
            return false;
        
        GameHandlers.StartGame(Context);
        GameHandlers.DequeueAllActions(Context);
        return true;
    }

    public bool EndRound()
    {
        var state = GameHandlers.EndRound(Context);
        GameHandlers.DequeueAllActions(Context);
        return state;
    }
}