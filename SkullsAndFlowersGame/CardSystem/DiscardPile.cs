using System.Diagnostics;

namespace SkullsAndFlowersGame.CardSystem;

public class DiscardPile : IDiscardPile
{
    private List<ICard> _cards = new();
    
    public string CollectionId { get; init; }
    public IEnumerable<ICard> Cards => _cards;
    
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
    
    public override string ToString()
    {
        return string.Join(", ", Cards);
    }
}