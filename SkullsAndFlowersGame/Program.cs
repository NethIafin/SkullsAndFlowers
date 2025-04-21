// See https://aka.ms/new-console-template for more information

using SkullsAndFlowersGame;
using SkullsAndFlowersGame.CardSystem;
using SkullsAndFlowersGame.CardSystem.Instances.Admin;
using SkullsAndFlowersGame.CardSystem.Instances.Flower.Expansion0;
using SkullsAndFlowersGame.CardSystem.Managers;
using SkullsAndFlowersGame.CardSystem.Mixins;
using SkullsAndFlowersGame.CardSystem.Mixins.Abstractions;

var game = new GameContext();
var gameHandler = new TurnManager()
{
    Context = game
};

var p1 = new Player();
var p2 = new Player();

var deck1 = new Deck();

deck1.AddCardToTop(new Tulip().GenerateCard());
deck1.AddCardToTop(new Tulip().GenerateCard());
deck1.AddCardToTop(new AnnihilatingLight().GenerateCard());
deck1.AddCardToTop(new Rose().GenerateCard());
deck1.AddCardToTop(new Daisy().GenerateCard());
deck1.AddCardToTop(new Oak().GenerateCard());
deck1.AddCardToTop(new RoyalTulip().GenerateCard());
deck1.AddCardToTop(new Tulip().GenerateCard());

var deck2 = new Deck();
deck2.AddCardToTop(new Tulip().GenerateCard());
deck2.AddCardToTop(new Tulip().GenerateCard());
deck2.AddCardToTop(new Tulip().GenerateCard());
deck2.AddCardToTop(new Tulip().GenerateCard());

game.AddPlayer(p1, deck1);
game.AddPlayer(p2, deck2);

foreach (var card in deck1.Cards)
{
    card.Owner = p1;
}

foreach (var card in deck2.Cards)
{
    card.Owner = p2;
}

gameHandler.WriteGameState();
gameHandler.DrawCards(6);
gameHandler.WriteGameState();

gameHandler.PlayCardByName("tulip");
gameHandler.WriteGameState();

gameHandler.PlayCardByNameTargetingOwnFieldCard("royal tulip", "tulip");
gameHandler.WriteGameState();

gameHandler.PlayCardByNameTargetingOwnHandCard("rose", "tulip");
gameHandler.WriteGameState();

gameHandler.PlayCardByName("oak");
gameHandler.WriteGameState();

gameHandler.PlayCardByName("daisy");
gameHandler.WriteGameState();

gameHandler.PlayCardByName("!!! annihilating light");
gameHandler.WriteGameState();

gameHandler.PlayCardByName("tulip");
gameHandler.WriteGameState();

GameHandlers.EndTurn(game, p1);

Console.WriteLine(game.PlayFields[0].GetOfType<FieldPowerMixin>().FirstOrDefault()?.Value.ToString());
