using System.Diagnostics;

namespace SkullsAndFlowersGame.CardSystem;

public class Card : ICard
{
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

    public IEnumerable<IMixin> AllMixins => _mixins;

    public string? Identifier { get; init; }
    public IPlayer? Owner { get; set; }
    public ICardContainer? Container { get; set; }

    public override string ToString()
    {
        return Identifier ?? "unknown card";
    }
}