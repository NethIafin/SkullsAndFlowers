using SkullsAndFlowersGame.CardSystem;
using SkullsAndFlowersGame.CardSystem.Managers;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame;

public static class GameStateHelper
{
    public static void WriteGameState(this TurnManager game)
    {
        var playerId = game.Context.ActivePlayer;
        Console.WriteLine($"State for Turn {game.Context.Turn}, Player {playerId}, Deck size {game.Context.Decks[playerId].Count}");
        Console.WriteLine($"\t Hand: {game.Context.PlayerHands[playerId]}");
        Console.WriteLine($"\t Field: {game.Context.PlayFields[playerId]}");
        Console.WriteLine($"\t Discard: {game.Context.DiscardPiles[playerId]}");
    }

    public static void PlayCardByName(this TurnManager game, string cardName)
    {
        var playerId = game.Context.ActivePlayer;
        ICard selectedCard = null;
        foreach (var card in game.Context.PlayerHands[playerId].Cards)
        {
            if(card.Identifier != cardName)
                continue;

            selectedCard = card;
            break;
        }
        
        if(selectedCard == null)
            return;
        
        GameHandlers.PlayCard(game.Context, game.Context.PlayerHands[playerId], game.Context.PlayFields[playerId], selectedCard, game.Context.Players[playerId]);
        game.DequeueAllActions();
    }

    public static void PlayCardByNameTargetingOwnFieldCard(this TurnManager game, string cardName,
        string fieldCardName)
    {
        var playerId = game.Context.ActivePlayer;
        ICard? sourceCard = null;
        ICard? targetCard = null;
        foreach (var card in game.Context.PlayerHands[playerId].Cards)
        {
            if(card.Identifier != cardName)
                continue;

            if(!card.GetOfType<ITargetCardMixin>().Any())
                continue;
            
            sourceCard = card;
            break;
        }
        
        foreach (var card in game.Context.PlayFields[playerId].Cards)
        {
            if(card.Identifier != fieldCardName)
                continue;
            
            targetCard = card;
            break;
        }
        
        if(targetCard == null || sourceCard == null)
            return;
        
        sourceCard.GetOfType<ITargetCardMixin>().FirstOrDefault().SetTarget(game.Context,
            game.Context.PlayFields[playerId], targetCard, sourceCard);
        GameHandlers.PlayCard(game.Context, game.Context.PlayerHands[playerId], game.Context.PlayFields[playerId], sourceCard, game.Context.Players[playerId]);
        game.DequeueAllActions();
    }
    
    public static void PlayCardByNameTargetingOwnHandCard(this TurnManager game, string cardName,
        string handCardName)
    {
        var playerId = game.Context.ActivePlayer;
        ICard? sourceCard = null;
        ICard? targetCard = null;
        foreach (var card in game.Context.PlayerHands[playerId].Cards)
        {
            if(card.Identifier != cardName)
                continue;

            if(!card.GetOfType<ITargetCardMixin>().Any())
                continue;
            
            sourceCard = card;
            break;
        }
        
        foreach (var card in game.Context.PlayerHands[playerId].Cards)
        {
            if(card.Identifier != handCardName)
                continue;
            
            targetCard = card;
            break;
        }
        
        if(targetCard == null || sourceCard == null)
            return;
        
        sourceCard.GetOfType<ITargetCardMixin>().FirstOrDefault().SetTarget(game.Context,
            game.Context.PlayerHands[playerId], targetCard, sourceCard);
        GameHandlers.PlayCard(game.Context, game.Context.PlayerHands[playerId], game.Context.PlayFields[playerId], sourceCard, game.Context.Players[playerId]);
        game.DequeueAllActions();
    }

    public static void DrawCards(this TurnManager game, int numberOfCards)
    {
        var playerId = game.Context.ActivePlayer;
        for(var i=0; i<numberOfCards;i++)
            game.Context.ScheduleDrawAction(game.Context.Players[playerId]);
        
        game.DequeueAllActions();
    }
}