namespace SkullsAndFlowersGame.CardSystem;

public interface IMixinContainer
{
    IEnumerable<T> GetOfType<T>() where T : class, IMixin;
    void AddMixin<T>(T mixin) where T : class, IMixin;
    IEnumerable<IMixin> AllMixins { get; }
}