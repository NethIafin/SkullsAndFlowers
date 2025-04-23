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
        builder.AddHandler<IsTokenMixin>();
        builder.AddHandler<DestroyCardOnPutItInHandMixin>();
        
        return builder.Build();
    }
    
    public static ICard DefaultGlobalCard(string name)
    {
        var card = new Card
        {
            Identifier = name
        };
        var builder = new MixinContainerBuilder<ICard>(card);

        builder.AddHandler<CardActiveMixin>();
        builder.AddHandler<GoToDiscardPileWhenRemovedMixin>();
        builder.AddHandler<GlobalCardCantBeTokenMixin>();
        builder.AddHandler<GlobalCardPlayBehaviorMixin>();
        
        return builder.Build();
    }

    public static ICard TokenizeCard(ICard sourceCard)
    {
        var card = new Card
        {
            Identifier = sourceCard.Identifier
        };
        var builder = new MixinContainerBuilder<ICard>(card);
        
        builder.AddHandler<DestroyCardOnPutItInHandMixin>();
        builder.AddHandler<IsTokenMixin>();

        foreach (var mixin in sourceCard.AllMixins)
        {
            if(mixin is GoToDiscardPileWhenRemovedMixin or DestroyCardOnPutItInHandMixin or IsTokenMixin)
                continue;

            builder.AddMixin(mixin);
        }

        return builder.Build();
    }
    
    public static ICard TokenizeCardWithCustomPower(ICard sourceCard, int customPower)
    {
        var card = new Card
        {
            Identifier = sourceCard.Identifier
        };
        var builder = new MixinContainerBuilder<ICard>(card);
        
        builder.AddHandler<DestroyCardOnPutItInHandMixin>();

        foreach (var mixin in sourceCard.AllMixins)
        {
            if (mixin is GoToDiscardPileWhenRemovedMixin)
                continue;

            if (mixin is CardPowerMixin)
            {
                builder.SetCardPower(customPower);
            }

            builder.AddMixin(mixin);
        }

        return builder.Build();
    }
}