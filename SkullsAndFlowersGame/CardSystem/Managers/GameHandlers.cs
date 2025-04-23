using System.Diagnostics;
using SkullsAndFlowersGame.CardSystem.Mixins;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;
using SkullsAndFlowersGame.CardSystem.ScheduledActions;

namespace SkullsAndFlowersGame.CardSystem.Managers;

public static partial class GameHandlers
{
    public static void PlayCard(GameContext context, IPlayField field, ICard card, IPlayer player)
    {
        card.Container?.RemoveCard(card);
        card.Container = null;
            
        field.AddCard(card);
        card.Container = field;

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
        
        foreach (var isCardActive in card.GetOfType<CardActiveMixin>())
        {
            if (!isCardActive.Value)
            {
                DiscardCard(context, card, player);
                break;
            }
        }
    }
    
    public static void PlaceCard(GameContext context, IPlayField field, ICard card, IPlayer player)
    {
        card.Container?.RemoveCard(card);
        card.Container = null;
        
        field.AddCard(card);
        card.Container = field;
    }
    

    public static void DiscardCard(GameContext context, ICard card, IPlayer player)
    {
        var success = card.Container?.RemoveCard(card) ?? false;
        card.Container = null;
        if (!success)
            return; // card already removed
        
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
    }
    
    public static void PutCardToDiscardPile(GameContext context, ICard card)
    {
        card.Container?.RemoveCard(card);
        card.Container = null;
        var playerId = card.Owner?.MatchPlayerId;
        if (playerId == null)
            return;

        context.DiscardPiles[playerId.Value].AddCard(card);
        card.Container = context.DiscardPiles[playerId.Value];
    }
    
    public static void PutCardToHand(GameContext context, ICard card)
    {
        card.Container?.RemoveCard(card);
        card.Container = null;
        var playerId = card.Owner?.MatchPlayerId;
        if (playerId == null)
            return;

        context.PlayerHands[playerId.Value].AddCard(card);
        card.Container = context.PlayerHands[playerId.Value];
    }
    
    public static void RemoveCard(GameContext context, ICard card)
    {
        var success = card.Container?.RemoveCard(card) ?? false;
        card.Container = null;
        if (!success)
            return; // card already removed
        
        foreach (var cardDiscardEffect in card.GetOfType<IRemoveCardMixin>())
        {
            cardDiscardEffect.OnRemoved(context, card);
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
        nextCard.Container = context.PlayerHands[playerId];
    }
    
    public static void StartGame(GameContext context)
    {
        foreach (var gameStartMixin in context.GetOfType<IGameStartMixin>())
        {
            gameStartMixin.OnGameStart(context);
        }
    }

    public static bool EndRound(GameContext context)
    {
        foreach (var endRoundMixin in context.GetOfType<IEndRoundMixin>())
        {
            if (!endRoundMixin.OnEndRound(context))
                return false;
        }

        return true;
    }

    public static void StandardWorldCleanup(GameContext context)
    {
        var cardsToRemove = new List<IEnumerable<ICard>>();

        for (var i = 0; i < context.PlayFields.Count; i++)
        {
            var result = new List<ICard>();
            var field = context.PlayFields[i];
            while (field.Count > 0)
            {
                var card = field.Cards.First();
                result.Add(card);
                field.RemoveCard(card);
            }
            cardsToRemove.Add(result);
        }

        for (var i = 0; i < context.PlayFields.Count; i++)
        {
            var cardsToRunRemoval = cardsToRemove[i];
            foreach (var card in cardsToRunRemoval)
            {
                foreach (var endRoundCardMixin in card.GetOfType<IEndRoundCardMixin>())
                {
                    endRoundCardMixin.OnRoundEnd(context, card);
                }
            }
        }

        foreach (var t in context.Players)
            t.Passed = false;
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
    
    const int CircuitBreakerAmount = 10000;

    public static void DequeueAllActions(GameContext context)
    {
        var circuitBreaker = 0;
        
        while (context.ScheduledActions.TryDequeue(out var action))
        {
            circuitBreaker++;
            switch (action)
            {
                case ScheduledPlayAction playAction :
                    PlayCard(context, playAction.Destination, playAction.Card, playAction.Player);
                    break;
                case ScheduledDiscardAction discardAction:
                    DiscardCard(context, discardAction.Card, discardAction.Player);
                    break;
                case ScheduledRemoveAction removeAction:
                    RemoveCard(context, removeAction.Card);
                    break;
                case ScheduledToDiscardPileAction discardAction:
                    PutCardToDiscardPile(context, discardAction.Card);
                    break;
                case ScheduledReturnToHandAction returnToHandAction:
                    RemoveCard(context, returnToHandAction.Card);
                    PutCardToHand(context, returnToHandAction.Card);
                    break;
                case ScheduleDrawAction drawAction:
                    DrawCard(context, drawAction.DrawingPlayer);
                    break;
                case ScheduledPlaceCard placeAction:
                    PlaceCard(context, context.PlayFields[placeAction.Player.MatchPlayerId] , placeAction.Card,  placeAction.Player);
                    break;
                default:
                    continue;
            }

            if (circuitBreaker > CircuitBreakerAmount)
            {
                Debug.WriteLine($"Circuit breaker with Action size of {context.ScheduledActions.Count}");
                context.ScheduledActions.Clear();
                break;
            }
        }
    }
}