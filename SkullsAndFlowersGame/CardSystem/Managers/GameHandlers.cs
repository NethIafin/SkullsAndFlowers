using System.Diagnostics;
using SkullsAndFlowersGame.CardSystem.Mixins;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Managers;

public static partial class GameHandlers
{
    public static void PlayCard(GameContext context, ICardContainer container, IPlayField field, ICard card, IPlayer player)
    {
        var success = container.RemoveCard(card);
        Debug.Assert(success, "card exists in container");

        SpawnCard(context, field, card, player);
    }
    
    public static void SpawnCard(GameContext context, IPlayField field, ICard card, IPlayer player)
    {
        field.AddCard(card);

        foreach (var cardPlayedEffect in EachCardOnFieldMatching<IOtherCardPlayed>(context))
        {
            cardPlayedEffect.OnOtherCardPlayed(context, card, player);
        }
        
        foreach (var cardPlayedEffect in EachCardInSharedMatching<IOtherCardPlayed>(context))
        {
            cardPlayedEffect.OnOtherCardPlayed(context, card, player);
        }

        foreach (var cardPlayedEffect in card.GetOfType<IPlayCardMixin>())
        {
            cardPlayedEffect.OnPlayed(context, card, field, player);
        }
        
        foreach (var cardAwareness in card.GetOfType<ICardFieldAwareMixin>())
        {
            cardAwareness.Field = field;
        }
        
        foreach (var isCardActive in card.GetOfType<CardActiveMixin>())
        {
            if (!isCardActive.Value)
            {
                DiscardCard(context, field, card, player);
                break;
            }
        }
    }

    public static void DiscardCard(GameContext context, ICardContainer container, ICard card, IPlayer player)
    {
        var success = container.RemoveCard(card);

        if (!success)
            return; // trying to discard it multiple times
        
        foreach (var cardDiscardedEffect in EachCardOnFieldMatching<IOtherCardDiscarded>(context))
        {
            cardDiscardedEffect.OnOtherCardDiscarded(context, card, player);
        }
        
        foreach (var cardDiscardedEffect in EachCardInSharedMatching<IOtherCardDiscarded>(context))
        {
            cardDiscardedEffect.OnOtherCardDiscarded(context, card, player);
        }
        
        foreach (var cardDiscardEffect in card.GetOfType<IDiscardCardMixin>())
        {
            cardDiscardEffect.OnDiscard(context, card);
        }
        
        foreach (var cardAwareness in card.GetOfType<ICardFieldAwareMixin>())
        {
            cardAwareness.Field = null;
        }
    }
    
    public static void PutCardToDiscardPile(GameContext context, ICard card)
    {
        var playerId = card.Owner?.MatchPlayerId;
        if (playerId == null)
            return;

        context.DiscardPiles[playerId.Value].AddCard(card);
    }
    
    public static void PutCardToHandPile(GameContext context, ICard card)
    {
        var playerId = card.Owner?.MatchPlayerId;
        if (playerId == null)
            return;

        context.PlayerHands[playerId.Value].AddCard(card);
    }
    
    public static void RemoveCard(GameContext context, ICardContainer container, ICard card)
    {
        var success = container.RemoveCard(card);
        if (!success)
            return; // card already removed
        
        foreach (var cardDiscardEffect in card.GetOfType<IRemoveCardMixin>())
        {
            cardDiscardEffect.OnRemoved(context, card);
        }
        
        foreach (var cardAwareness in card.GetOfType<ICardFieldAwareMixin>())
        {
            cardAwareness.Field = null;
        }
    }

    public static void DrawCard(GameContext context, IPlayer player)
    {
        var playerId = player.MatchPlayerId;

        var nextCard = context.Decks[playerId].Draw();

        if (nextCard == null)
        {
            return;
        }

        context.PlayerHands[playerId].AddCard(nextCard);
    }
    
    public static void StartGame(GameContext context)
    {
        foreach (var gameStartMixin in context.GetOfType<IGameStartMixin>())
        {
            gameStartMixin.OnGameStart(context);
        }
    }
    
    public static void EndTurn(GameContext context, IPlayer activePlayer)
    {
        foreach (var cardPlayedEffect in EachCardOnFieldMatchingWithCard<IEndTurnCardMixin>(context))
        {
            cardPlayedEffect.Item2.OnTurnEnd(context, cardPlayedEffect.Item1, activePlayer);
        }
        
        foreach (var cardPlayedEffect in EachCardInSharedMatchingWithCard<IEndTurnCardMixin>(context))
        {
            cardPlayedEffect.Item2.OnTurnEnd(context, cardPlayedEffect.Item1, activePlayer);
        }

        foreach (var fieldEffect in EachFieldMatchingWithField<IEndTurnFieldMixin>(context))
        {
            fieldEffect.Item2.OnTurnEnd(context, fieldEffect.Item1, activePlayer);
        }
    }
    
    public static void StartTurn(GameContext context, IPlayer activePlayer)
    {
        foreach (var cardPlayedEffect in EachCardOnFieldMatchingWithCard<IStartTurnCardMixin>(context))
        {
            cardPlayedEffect.Item2.OnTurnStart(context, cardPlayedEffect.Item1, activePlayer);
        }
        
        foreach (var cardPlayedEffect in EachCardInSharedMatchingWithCard<IStartTurnCardMixin>(context))
        {
            cardPlayedEffect.Item2.OnTurnStart(context, cardPlayedEffect.Item1, activePlayer);
        }
    }
}