﻿using System.Diagnostics;
using SkullsAndFlowersGame.CardSystem.ScheduledActions;

namespace SkullsAndFlowersGame.CardSystem;

public class GameContext : IMixinContainer
{
    public int ActivePlayer { get; set; } = 0;
    public int Turn { get; set; } = 0;
    public List<IPlayer> Players { get; } = new ();
    public List<IPlayField> PlayFields { get; } = new ();
    public List<IDeck> Decks { get; } = new ();
    public List<IPlayerHand> PlayerHands { get; } = new ();
    public List<IDiscardPile> DiscardPiles { get; } = new ();
    public ISharedField SharedField { get; } = DefaultsFactory.DefaultSharedField();

    public Queue<IPlannedAction> ScheduledActions { get; }= new ();
    

    public void AddPlayer(IPlayer player, IDeck deck, IDiscardPile pile, IPlayField playField, IPlayerHand playerHand)
    {
        Players.Add(player);
        player.MatchPlayerId = Players.Count - 1;
        player.Passed = false;
        player.Score = 0;
        Decks.Add(deck);
        DiscardPiles.Add(pile);
        PlayFields.Add(playField);
        PlayerHands.Add(playerHand);
    }

    public void AddPlayer(IPlayer player, IDeck deck)
    {
        AddPlayer(player, deck, DefaultsFactory.DefaultDiscard(), DefaultsFactory.DefaultPlayField(), DefaultsFactory.DefaultPlayerHand());
    }

    public void SchedulePlayAction(ICard card, IPlayField field, IPlayer player)
    {
        ScheduledActions.Enqueue(new ScheduledPlayAction { Card = card, Destination = field, Player = player });
    }
    
    public void ScheduleDiscardAction(ICard card, IPlayer player)
    {
        ScheduledActions.Enqueue(new ScheduledDiscardAction { Card = card, Player = player });
    }
    
    public void ScheduleToDiscardPileAction(ICard card)
    {
        ScheduledActions.Enqueue(new ScheduledToDiscardPileAction() { Card = card });
    }
    
    public void ScheduleReturnToHandAction(ICard card, ICardContainer container)
    {
        ScheduledActions.Enqueue(new ScheduledReturnToHandAction() { Card = card, Source = container});
    }
    
    public void ScheduleDrawAction(IPlayer player)
    {
        ScheduledActions.Enqueue(new ScheduleDrawAction() { DrawingPlayer = player});
    }
    
    public void SchedulePlaceAction(ICard card, IPlayer player)
    {
        ScheduledActions.Enqueue(new ScheduledPlaceCard() { Player = player, Card = card});
    }
    
    private readonly List<IMixin> _mixins = new();
    private readonly Dictionary<Type, IEnumerable<IMixin>> _cachedMixins = new();
    
    public IEnumerable<IMixin> AllMixins => _mixins;

    public void AddMixin<T>(T mixin) where T : class, IMixin
    {
        Debug.Assert(mixin != null, $"Mixin has to be not null");
        
        _mixins.Add(mixin);
        
        _cachedMixins.Clear();
    }
    
    public IEnumerable<T> GetOfType<T>() where T : class, IMixin
    {
        if (_cachedMixins.TryGetValue(typeof(T), out var cache))
        {
            return cache.Cast<T>();
        }

        var result = _mixins.Where(x => x is T).ToArray();
        _cachedMixins.Add(typeof(T), result);

        return result.Cast<T>();
    }
}