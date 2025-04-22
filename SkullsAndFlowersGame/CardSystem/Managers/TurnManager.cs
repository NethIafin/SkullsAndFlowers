using System.Diagnostics;
using SkullsAndFlowersGame.CardSystem.ScheduledActions;

namespace SkullsAndFlowersGame.CardSystem.Managers;

public class TurnManager
{
    public GameContext Context { get; set; }
    const int CircuitBreakerAmount = 10000;

    public void DequeueAllActions()
    {
        var circuitBreaker = 0;
        
        while (Context.ScheduledActions.TryDequeue(out var action))
        {
            circuitBreaker++;
            switch (action)
            {
                case ScheduledPlayAction playAction :
                    GameHandlers.SpawnCard(Context, playAction.Destination, playAction.Card, playAction.Player);
                    break;
                case ScheduledDiscardAction discardAction:
                    GameHandlers.DiscardCard(Context, discardAction.Source, discardAction.Card, discardAction.Player);
                    break;
                case ScheduledRemoveAction removeAction:
                    GameHandlers.RemoveCard(Context, removeAction.Source, removeAction.Card);
                    break;
                case ScheduledToDiscardPileAction discardAction:
                    GameHandlers.PutCardToDiscardPile(Context, discardAction.Card);
                    break;
                case ScheduledReturnToHandAction returnToHandAction:
                    GameHandlers.RemoveCard(Context, returnToHandAction.Source, returnToHandAction.Card);
                    GameHandlers.PutCardToHandPile(Context, returnToHandAction.Card);
                    break;
                case ScheduleDrawAction drawAction:
                    GameHandlers.DrawCard(Context, drawAction.DrawingPlayer);
                    break;
                default:
                    continue;
            }

            if (circuitBreaker > CircuitBreakerAmount)
            {
                Debug.WriteLine($"Circuit breaker with Action size of {Context.ScheduledActions.Count}");
                Context.ScheduledActions.Clear();
                break;
            }
        }
    }

    public bool EndTurn()
    {
        GameHandlers.EndTurn(Context, Context.Players[Context.ActivePlayer]);
        DequeueAllActions();
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
        GameHandlers.StartTurn(Context, Context.Players[Context.ActivePlayer]);
        DequeueAllActions();
        
        if (Context.Players[Context.ActivePlayer].Passed)
            return false;

        return true;
    }

    public bool StartGame()
    {
        if (Context.Players.Count < 2)
            return false;
        
        GameHandlers.StartGame(Context);
        DequeueAllActions();
        return true;
    }

    public void EndRound()
    {
        
    }
}