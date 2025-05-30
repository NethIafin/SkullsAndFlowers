﻿using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

namespace SkullsAndFlowersGame.CardSystem.Mixins;

public class SpawnTokenWhenPlayedMixin<T> : IPlayCardMixin where T : ICardTemplate, new()
{
    public string MixinId => "Spawn Token When Played";
    public void OnPlayed(GameContext context, ICard playedCard,  IPlayField field, IPlayer playedPlayer)
    {
        var tokenTemplate = new T();
        var token = tokenTemplate.GenerateCard();
        token.Owner = playedPlayer;
        context.SchedulePlaceAction(token ,playedPlayer);
    }
}