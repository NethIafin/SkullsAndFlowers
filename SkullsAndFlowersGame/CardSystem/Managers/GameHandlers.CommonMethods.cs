namespace SkullsAndFlowersGame.CardSystem.Managers;

public static partial class GameHandlers
{
    public static IEnumerable<T> EachCardOnFieldMatching<T>(GameContext context) where T : class, IMixin
    {
        return context.PlayFields
            .SelectMany(x => x.Cards
                .SelectMany(y => y.GetOfType<T>()));
    }
    
    public static IEnumerable<Tuple<ICard,T>> EachCardOnFieldMatchingWithCard<T>(GameContext context) where T : class, IMixin
    {
        return context.PlayFields
            .SelectMany(x => x.Cards
                .SelectMany(y => y.GetOfType<T>().Select(z=>new Tuple<ICard, T>(y,z))));
    }
    
    public static IEnumerable<T> EachCardInSharedMatching<T>(GameContext context) where T : class, IMixin
    {
        return context.SharedField.Cards
            .SelectMany(y => y.GetOfType<T>());
    }
    
    public static IEnumerable<Tuple<ICard,T>> EachCardInSharedMatchingWithCard<T>(GameContext context) where T : class, IMixin
    {
        return context.SharedField.Cards
                .SelectMany(y => y.GetOfType<T>().Select(z=>new Tuple<ICard, T>(y,z)));
    }
    
    public static IEnumerable<Tuple<IPlayField,T>> EachFieldMatchingWithField<T>(GameContext context) where T : class, IMixin
    {
        return context.PlayFields
            .SelectMany(y => y.GetOfType<T>().Select(z=>new Tuple<IPlayField, T>(y,z)));
    }
}