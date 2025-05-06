using System.Collections.Concurrent;
using SkullsAndFlowersGame;
using SkullsAndFlowersGame.CardSystem;
using SkullsAndFlowersGame.CardSystem.Instances.Mutators.WorldStart;
using SkullsAndFlowersGame.CardSystem.Managers;
using SkullsAndFlowersGame.CardSystem.Mixins;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersWeb.Services;

public class GameService
{
    private readonly ConcurrentDictionary<string, GameInstance> _games = new();

    public class GameInstance
    {
        public TurnManager GameHandler { get; set; }
        public Dictionary<int, string> PlayerIdentifiers { get; set; } = new();
        public List<string> GameLog { get; } = new();
        public bool IsGameStarted { get; set; }
        public bool IsRoundEnded { get; set; }

        public void AddLogMessage(string message)
        {
            GameLog.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            if (GameLog.Count > 100)
            {
                GameLog.RemoveAt(0);
            }
        }
    }

    public GameInstance GetOrCreateGame(string gameId)
    {
        return _games.GetOrAdd(gameId, _ =>
        {
            var game = new DefaultWorld().PrepareWorld();
            var gameHandler = new TurnManager
            {
                Context = game
            };
            
            return new GameInstance
            {
                GameHandler = gameHandler,
                IsGameStarted = false,
                IsRoundEnded = false
            };
        });
    }

    public bool TryGetGame(string gameId, out GameInstance game)
    {
        return _games.TryGetValue(gameId, out game);
    }

    public bool SetupNewGame(string gameId, IDeck deck1, IDeck deck2)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        if (gameInstance.IsGameStarted)
            return false;

        var p1 = GameStateHelper.GeneratePlayerForDeck(deck1);
        var p2 = GameStateHelper.GeneratePlayerForDeck(deck2);
        
        gameInstance.GameHandler.Context.AddPlayer(p1, deck1);
        gameInstance.GameHandler.Context.AddPlayer(p2, deck2);

        // Assign player identifiers
        gameInstance.PlayerIdentifiers[p1.MatchPlayerId] = "Player 1";
        gameInstance.PlayerIdentifiers[p2.MatchPlayerId] = "Player 2";
        
        return true;
    }

    public bool StartGame(string gameId)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        if (gameInstance.IsGameStarted)
            return false;

        gameInstance.GameHandler.StartGame();
        gameInstance.IsGameStarted = true;
        gameInstance.AddLogMessage("Game started");
        
        return true;
    }

    public bool StartTurn(string gameId)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        if (!gameInstance.IsGameStarted)
            return false;

        var canStartTurn = true;

        while (true)
        {
            canStartTurn = gameInstance.GameHandler.StartTurn();
            var activePlayerId = gameInstance.GameHandler.Context.ActivePlayer;

            if (!canStartTurn)
            {
                gameInstance.AddLogMessage($"Player {activePlayerId} already passed.");
                var canEndTurn = gameInstance.GameHandler.EndTurn();

                if (!canEndTurn)
                {
                    gameInstance.AddLogMessage("All players passed, round end.");
                    gameInstance.IsRoundEnded = true;
                    return false;
                }
            }
            else
            {
                gameInstance.AddLogMessage(
                    $"Turn {gameInstance.GameHandler.Context.Turn}, Player {activePlayerId}'s turn");
                return true;
            }
        }
    }

    public bool EndTurn(string gameId)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        if (!gameInstance.IsGameStarted)
            return false;

        var activePlayerId = gameInstance.GameHandler.Context.ActivePlayer;
        gameInstance.GameHandler.EndTurn();
        
        var currentPower = gameInstance.GameHandler.Context.PlayFields[activePlayerId].GetOfType<FieldPowerMixin>()
            .FirstOrDefault()?.Value.ToString();
        
        gameInstance.AddLogMessage($"Player {activePlayerId} ended turn. Total Power: {currentPower}");
        
        return true;
    }

    public bool EndRound(string gameId)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        if (!gameInstance.IsGameStarted || !gameInstance.IsRoundEnded)
            return false;

        var result = gameInstance.GameHandler.EndRound();
        
        if (!result)
        {
            gameInstance.AddLogMessage("Game over!");
            return false;
        }
        
        gameInstance.IsRoundEnded = false;
        gameInstance.AddLogMessage("Round ended. New round starting.");
        
        var scoreSummary = string.Join(", ", gameInstance.GameHandler.Context.Players.Select(x => 
            $"Player {x.MatchPlayerId}: {x.Score}"));
        gameInstance.AddLogMessage($"Current score: {scoreSummary}");
        
        return true;
    }

    public bool PlayCard(string gameId, int cardIndex)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        var playerId = gameInstance.GameHandler.Context.ActivePlayer;
        var hand = gameInstance.GameHandler.Context.PlayerHands[playerId];
        
        if (cardIndex < 0 || cardIndex >= hand.Cards.Count())
            return false;
        
        ICard selectedCard = hand.Cards.ToList()[cardIndex];
        
        if (selectedCard.GetOfType<ITargetCardMixin>().Any(mixin => mixin.TargetCard == null))
        {
            gameInstance.AddLogMessage("Card requires target");
            return false;
        }
        
        GameHandlers.PlayCard(
            gameInstance.GameHandler.Context, 
            gameInstance.GameHandler.Context.PlayFields[playerId], 
            selectedCard, 
            gameInstance.GameHandler.Context.Players[playerId]
        );
        
        gameInstance.AddLogMessage($"Player {playerId} played {selectedCard.Identifier}");
        GameHandlers.DequeueAllActions(gameInstance.GameHandler.Context);
        EndTurn(gameId);
        StartTurn(gameId);
        
        return true;
    }

    public bool PlayCardTargetingFieldCard(string gameId, int cardIndex, int targetIndex)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        var playerId = gameInstance.GameHandler.Context.ActivePlayer;
        var hand = gameInstance.GameHandler.Context.PlayerHands[playerId];
        var field = gameInstance.GameHandler.Context.PlayFields[playerId];
        
        if (cardIndex < 0 || cardIndex >= hand.Cards.Count())
            return false;
        
        if (targetIndex < 0 || targetIndex >= field.Cards.Count())
            return false;
        
        ICard sourceCard = hand.Cards.ToList()[cardIndex];
        ICard targetCard = field.Cards.ToList()[targetIndex];
        
        var targetMixin = sourceCard.GetOfType<ITargetCardMixin>()
            .FirstOrDefault(mixin => mixin.TargetCard == null);
        
        if (targetMixin == null)
        {
            gameInstance.AddLogMessage("Card doesn't require targeting");
            return false;
        }
        
        var successfullySet = targetMixin.SetTarget(
            gameInstance.GameHandler.Context,
            field, 
            targetCard, 
            sourceCard
        );
        
        if (!successfullySet)
        {
            gameInstance.AddLogMessage("Invalid target");
            return false;
        }
        
        var extraTargetMixin = sourceCard.GetOfType<ITargetCardMixin>()
            .FirstOrDefault(mixin => mixin.TargetCard == null);
        
        if (extraTargetMixin != null)
        {
            gameInstance.AddLogMessage("Needs additional targets");
            return false;
        }
        
        GameHandlers.PlayCard(
            gameInstance.GameHandler.Context, 
            gameInstance.GameHandler.Context.PlayFields[playerId], 
            sourceCard, 
            gameInstance.GameHandler.Context.Players[playerId]
        );
        
        gameInstance.AddLogMessage($"Player {playerId} played {sourceCard.Identifier} targeting field card {targetCard.Identifier}");
        GameHandlers.DequeueAllActions(gameInstance.GameHandler.Context);
        EndTurn(gameId);
        StartTurn(gameId);
        
        return true;
    }

    public bool PlayCardTargetingHandCard(string gameId, int cardIndex, int targetIndex)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        var playerId = gameInstance.GameHandler.Context.ActivePlayer;
        var hand = gameInstance.GameHandler.Context.PlayerHands[playerId];
        
        if (cardIndex < 0 || cardIndex >= hand.Cards.Count())
            return false;
        
        if (targetIndex < 0 || targetIndex >= hand.Cards.Count() || cardIndex == targetIndex)
            return false;
        
        ICard sourceCard = hand.Cards.ToList()[cardIndex];
        ICard targetCard = hand.Cards.ToList()[targetIndex];
        
        var targetMixin = sourceCard.GetOfType<ITargetCardMixin>()
            .FirstOrDefault(mixin => mixin.TargetCard == null);
        
        if (targetMixin == null)
        {
            gameInstance.AddLogMessage("Card doesn't require targeting");
            return false;
        }
        
        var successfullySet = targetMixin.SetTarget(
            gameInstance.GameHandler.Context,
            hand, 
            targetCard, 
            sourceCard
        );
        
        if (!successfullySet)
        {
            gameInstance.AddLogMessage("Invalid target");
            return false;
        }
        
        var extraTargetMixin = sourceCard.GetOfType<ITargetCardMixin>()
            .FirstOrDefault(mixin => mixin.TargetCard == null);
        
        if (extraTargetMixin != null)
        {
            gameInstance.AddLogMessage("Needs additional targets");
            return false;
        }
        
        GameHandlers.PlayCard(
            gameInstance.GameHandler.Context, 
            gameInstance.GameHandler.Context.PlayFields[playerId], 
            sourceCard, 
            gameInstance.GameHandler.Context.Players[playerId]
        );
        
        gameInstance.AddLogMessage($"Player {playerId} played {sourceCard.Identifier} targeting hand card {targetCard.Identifier}");
        GameHandlers.DequeueAllActions(gameInstance.GameHandler.Context);
        EndTurn(gameId);
        StartTurn(gameId);
        
        return true;
    }

    public bool PassTurn(string gameId)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        var activePlayerId = gameInstance.GameHandler.Context.ActivePlayer;
        gameInstance.GameHandler.Context.Players[activePlayerId].Passed = true;
        
        gameInstance.AddLogMessage($"Player {activePlayerId} passed");
        
        var canEndTurn = gameInstance.GameHandler.EndTurn();
        if (!canEndTurn)
        {
            gameInstance.AddLogMessage("All players passed, round end.");
            gameInstance.IsRoundEnded = true;
        }
        
        return true;
    }

    public bool DrawCard(string gameId)
    {
        if (!TryGetGame(gameId, out var gameInstance))
            return false;

        var activePlayerId = gameInstance.GameHandler.Context.ActivePlayer;
        gameInstance.GameHandler.Context.ScheduleDrawAction(gameInstance.GameHandler.Context.Players[activePlayerId]);
        GameHandlers.DequeueAllActions(gameInstance.GameHandler.Context);
        
        gameInstance.AddLogMessage($"Player {activePlayerId} drew a card");
        
        return true;
    }
}