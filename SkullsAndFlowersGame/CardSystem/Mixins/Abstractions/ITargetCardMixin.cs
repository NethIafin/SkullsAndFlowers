namespace SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

public interface ITargetCardMixin : IMixin
{
    ICard? TargetCard { get; set; }
    ICardContainer? TargetCardContainer { get; set; }
    TargetingFlags ValidTarget { get; }

    bool SetTarget(GameContext context, ICardContainer targetCardSource, ICard targetCard, ICard thisCard);
}