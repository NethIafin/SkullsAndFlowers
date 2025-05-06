using System.Reflection;
using SkullsAndFlowersGame.CardSystem;
using SkullsAndFlowersGame.CardSystem.Managers;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame;

public static class GameStateHelper
{
    private static Dictionary<string, ICardTemplate> _cards = new();

    public static void InitCards()
    {
        if (_cards.Count > 0)
            return;
        
        var templates = FindAndInstantiateImplementations<ICardTemplate>();

        foreach (var template in templates)
        {
            _cards.Add(template.GenerateCard().Identifier!, template);
        }
    }
    
    public static void WriteGameState(this TurnManager game)
    {
        var playerId = game.Context.ActivePlayer;
        Console.WriteLine("");
        Console.WriteLine($"State for Turn {game.Context.Turn}, Player {playerId}, Deck size {game.Context.Decks[playerId].Count}");
        Console.WriteLine($"\t\t Global: {game.Context.SharedField}");
        Console.WriteLine($"\t Hand: {game.Context.PlayerHands[playerId]}");
        Console.WriteLine($"\t Field: {game.Context.PlayFields[playerId]}");
        Console.WriteLine($"\t Discard: {game.Context.DiscardPiles[playerId]}");
    }
    
    public static void WriteScore(this TurnManager game)
    {
        Console.WriteLine("\t\tCurrent score:");
        Console.WriteLine($"\t\t{string.Join(", ", game.Context.Players.Select(x=>$"Player {x.MatchPlayerId}: {x.Score}"))}");
    }

    public static bool PlayCardByName(this TurnManager game, string cardName)
    {
        var playerId = game.Context.ActivePlayer;
        ICard? selectedCard = game.Context.PlayerHands[playerId].Cards.FirstOrDefault(card => card.Identifier == cardName);

        if(selectedCard == null)
            return false;

        if (selectedCard.GetOfType<ITargetCardMixin>().Any(mixin => mixin.TargetCard == null))
        {
            Console.WriteLine("\tCard requires target");
            return false;
        }
        
        GameHandlers.PlayCard(game.Context, game.Context.PlayFields[playerId], selectedCard, game.Context.Players[playerId]);
        
        return true;
    }

    public static bool PlayCardByNameTargetingOwnFieldCard(this TurnManager game, string cardName,
        string fieldCardName)
    {
        var playerId = game.Context.ActivePlayer;
        ICard? sourceCard = game.Context.PlayerHands[playerId].Cards.Where(card => card.Identifier == cardName)
            .FirstOrDefault(card => card.GetOfType<ITargetCardMixin>().Any(mixin => mixin.TargetCard == null));

        ICard? targetCard = game.Context.PlayFields[playerId].Cards.FirstOrDefault(card => card.Identifier == fieldCardName);

        if(targetCard == null || sourceCard == null)
        {
            Console.WriteLine("\tUnable to target");
            return false;
        }
        
        var successfullySet = sourceCard.GetOfType<ITargetCardMixin>().FirstOrDefault(mixin => mixin.TargetCard == null)
            ?.SetTarget(game.Context,
            game.Context.PlayFields[playerId], targetCard, sourceCard);
        
        if (!(successfullySet ?? false))
        {
            Console.WriteLine("\tInvalid target");
            return false;
        }
        
        GameHandlers.PlayCard(game.Context, game.Context.PlayFields[playerId], sourceCard, game.Context.Players[playerId]);

        return true;
    }
    
    public static bool PlayCardByNameTargetingOwnHandCard(this TurnManager game, string cardName,
        string handCardName)
    {
        var playerId = game.Context.ActivePlayer;
        var sourceCard = game.Context.PlayerHands[playerId].Cards.Where(card => card.Identifier == cardName)
            .FirstOrDefault(card => card.GetOfType<ITargetCardMixin>().Any(mixin => mixin.TargetCard == null));
        
        var targetCard = game.Context.PlayerHands[playerId].Cards.FirstOrDefault(card => card.Identifier == handCardName);

        if (targetCard == null || sourceCard == null)
        {
            Console.WriteLine("\tUnable to target");
            return false;
        }

        var successfullySet = sourceCard.GetOfType<ITargetCardMixin>().FirstOrDefault(mixin => mixin.TargetCard == null)
            ?.SetTarget(game.Context,
            game.Context.PlayerHands[playerId], targetCard, sourceCard);

        if (!(successfullySet ?? false))
        {
            Console.WriteLine("\tInvalid target");
            return false;
        }
        
        GameHandlers.PlayCard(game.Context, game.Context.PlayFields[playerId], sourceCard, game.Context.Players[playerId]);

        return true;
    }

    public static void DrawCards(this TurnManager game, int numberOfCards)
    {
        var playerId = game.Context.ActivePlayer;
        for(var i=0; i<numberOfCards;i++)
            game.Context.ScheduleDrawAction(game.Context.Players[playerId]);
    }

    public static IDeck GenerateDeck(params string[] cardNames)
    {
        if(_cards.Count == 0)
            InitCards();

        var deck = new Deck();

        foreach (var card in cardNames)
        {
            var split = card.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var toAddCount = 1;
            if (split.Length > 1)
                toAddCount = int.Parse(split[1]);
            for(var i=0; i<toAddCount; i++)
                deck.AddCard(_cards[split[0]].GenerateCard());
        }
        
        deck.Shuffle();

        return deck;
    }

    public static ICard GenerateCard(string cardName)
    {
        if(_cards.Count == 0)
            InitCards();
        return _cards[cardName].GenerateCard();
    }

    public static IPlayer GeneratePlayerForDeck(IDeck deckToOwn)
    {
        var player = new Player();

        foreach (var card in deckToOwn.Cards)
        {
            card.Owner = player;
        }

        return player;
    }

    public static bool ConsoleActionHelper(this TurnManager manager)
    {
        var validAction = false;
        while (!validAction)
        {
            Console.WriteLine("Enter Action");
            Console.Write(">> ");
            var action = Console.ReadLine();
            if (action == "exit")
                return false;
            if (action == "pass")
            {
                manager.Context.Players[manager.Context.ActivePlayer].Passed = true;
                var canEndTurn = manager.EndTurn();
                if (!canEndTurn)
                {
                    Console.WriteLine($"\tAll players passed, round end.");
                    return false;
                }

                return true;
            }

            var actionParts =
                action!.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (actionParts.Length == 3)
            {
                switch (actionParts[1])
                {
                    case "hand": validAction = manager.PlayCardByNameTargetingOwnHandCard(actionParts[0], actionParts[2]);
                        break;
                    case "field": validAction = manager.PlayCardByNameTargetingOwnFieldCard(actionParts[0], actionParts[2]);
                        break;
                }
            }
            else
            {
                validAction = manager.PlayCardByName(actionParts[0]);
            }
        }

        GameHandlers.DequeueAllActions(manager.Context);

        return true;
    }
    
    
    
    
    public static List<TInterface> FindAndInstantiateImplementations<TInterface>(Assembly assembly = null)
    {
        // If no assembly specified, use the calling assembly
        if (assembly == null)
        {
            assembly = Assembly.GetCallingAssembly();
        }

        var result = new List<TInterface>();
        var interfaceType = typeof(TInterface);

        // Check that TInterface is actually an interface
        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"Type {interfaceType.Name} is not an interface", nameof(TInterface));
        }

        try
        {
            // Find all non-abstract classes that implement the interface
            var implementingTypes = assembly.GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && // Implements the interface
                            t.IsClass &&                         // Is a class
                            !t.IsAbstract &&                     // Is not abstract
                            !t.IsInterface &&                    // Is not an interface
                            t.GetConstructor(Type.EmptyTypes) != null); // Has parameterless constructor

            // Instantiate each type using its parameterless constructor
            foreach (var type in implementingTypes)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    result.Add((TInterface)instance);
                    Console.WriteLine($"Successfully instantiated: {type.FullName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to instantiate {type.FullName}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching for implementations: {ex.Message}");
        }

        return result;
    }
}