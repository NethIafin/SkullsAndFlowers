﻿using SkullsAndFlowersGame.CardSystem.Mixins;

namespace SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;

public class Daisy : ICardTemplate
{
    public ICard GenerateCard()
    {
        return MixinContainerBuilder.StartCard("daisy")
            .SetCardPower(1)
            .AddSelflessBuffAbility(1)
            .Build();
    }
}