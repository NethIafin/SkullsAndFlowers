using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem;

public static partial class DefaultsFactory
{
    public static IDiscardPile DefaultDiscard()
    {
        return new DiscardPile();
    }
    
    public static IPlayField DefaultPlayField()
    {
        var playField = new PlayField()
        {
            CollectionId = Guid.NewGuid().ToString()
        };
        var builder = new MixinContainerBuilder<IPlayField>(playField);

        builder.AddHandler<FieldPowerMixin>();

        return builder.Build();
    }
    
    public static ISharedField DefaultSharedField()
    {
        return new SharedField();
    }
    
    public static IPlayerHand DefaultPlayerHand()
    {
        return new PlayerHand();
    }
    
    public static ICard DefaultPlayCard(string name)
    {
        var card = new Card
        {
            Identifier = name
        };
        var builder = new MixinContainerBuilder<ICard>(card);

        builder.AddHandler<CardActiveMixin>();
        builder.AddHandler<GoToDiscardPileWhenRemovedMixin>();
        
        return builder.Build();
    }
    
    public static ICard DefaultTokenCard(string name)
    {
        var card = new Card
        {
            Identifier = name
        };
        var builder = new MixinContainerBuilder<ICard>(card);

        builder.AddHandler<CardActiveMixin>();
        
        return builder.Build();
    }
}