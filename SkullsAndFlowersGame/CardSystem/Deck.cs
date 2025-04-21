using System.Diagnostics;

namespace SkullsAndFlowersGame.CardSystem;

public class Deck : IDeck
{
    private List<ICard> _cards = new();
    
    public string? CollectionId { get; init; }
    public IEnumerable<ICard> Cards => _cards; 
    public bool AddCardToTop(ICard card)
    {
        Debug.Assert(card != null, "card != null");
        
        _cards.Add(card);
        return true;
    }

    public bool AddCardToBottom(ICard card)
    {
        Debug.Assert(card != null, "card != null");
        
        _cards.Insert(0, card);
        return true;
    }

    public bool AddCard(ICard card)
    {
        Debug.Assert(card != null, "card != null");
        
        _cards.Add(card);
        return true;
    }

    public bool RemoveCard(ICard card)
    {
        Debug.Assert(card != null, "card != null");

        return _cards.Remove(card);
    }

    public int Count => _cards.Count;
    
    public ICard? Draw()
    {
        if (_cards.Count < 1) return null;
        
        var toReturn = _cards[^1];
        _cards.RemoveAt(_cards.Count - 1);
        return toReturn;
    }

    public void Shuffle()
    {
        var rnd = new Random();
        _cards = _cards.OrderBy(_ => rnd.Next()).ToList();
    }
    
    private readonly List<IMixin> _mixins = new();
    private readonly Dictionary<Type, IEnumerable<IMixin>> _cachedMixins = new();

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