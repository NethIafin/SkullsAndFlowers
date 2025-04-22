namespace SkullsAndFlowersGame.CardSystem;

public class MixinContainerBuilder<T>(T buildableItem) where T : IMixinContainer
{
    public T Item => buildableItem;
    public MixinContainerBuilder<T> AddMixin<TM>(TM mixin) where TM : class, IMixin
    {
        buildableItem.AddMixin(mixin);
        if (mixin is IMixinContainerAware)
        {
            (mixin as IMixinContainerAware).Container = buildableItem;
        }
        return this;
    }
    
    public MixinContainerBuilder<T> AddHandler<TM>() where TM : class, IMixin, new()
    {
        var mixin = new TM();
        buildableItem.AddMixin(mixin);
        if (mixin is IMixinContainerAware)
        {
            (mixin as IMixinContainerAware).Container = buildableItem;
        }
        return this;
    }

    public T Build()
    {
        return buildableItem;
    }
}

public static class MixinContainerBuilder
{
    public static MixinContainerBuilder<ICard> StartCard(string name)
    {
        var card = DefaultsFactory.DefaultPlayCard(name);
        return new MixinContainerBuilder<ICard>(card);
    }
    
    public static MixinContainerBuilder<ICard> StartToken(string name)
    {
        var card = DefaultsFactory.DefaultTokenCard(name);
        return new MixinContainerBuilder<ICard>(card);
    }

    public static MixinContainerBuilder<GameContext> StartWorld()
    {
        var world = new GameContext();
        return new MixinContainerBuilder<GameContext>(world);
    }

    public static MixinContainerBuilder<GameContext> AddPlayerWithDeck(this MixinContainerBuilder<GameContext> builder, IPlayer player, IDeck deck)
    {
        builder.Item.AddPlayer(player, deck);
        return builder;
    }
}