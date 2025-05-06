// See https://aka.ms/new-console-template for more information

using SkullsAndFlowersGame;
using SkullsAndFlowersGame.CardSystem.Instances.Mutators.WorldStart;
using SkullsAndFlowersGame.CardSystem.Managers;
using SkullsAndFlowersGame.CardSystem.Mixins;

var game = new DefaultWorld().PrepareWorld();
var gameHandler = new TurnManager()
{
    Context = game
};

var deck1 = GameStateHelper.GenerateDeck("tulip:3", "daisy:3", "rose:3", "clear skies:3", "oak:3");
var deck2 = GameStateHelper.GenerateDeck("tulip:3", "daisy:3", "rose:3", "soil fertilizing:3", "oak:3");

var p1 = GameStateHelper.GeneratePlayerForDeck(deck1);
var p2 = GameStateHelper.GeneratePlayerForDeck(deck2);

game.AddPlayer(p1, deck1);
game.AddPlayer(p2, deck2);

gameHandler.StartGame();

var shouldTerminate = false;
var shouldTerminateRound = false;

while (!shouldTerminate)
{
    while (!shouldTerminate || !shouldTerminateRound)
    {
        var canStartTurn = gameHandler.StartTurn();

        var activePlayerId = gameHandler.Context.ActivePlayer;

        if (!canStartTurn)
        {
            Console.WriteLine($"Player {activePlayerId} already passed.");
            var canEndTurn = gameHandler.EndTurn();
            if (!canEndTurn)
            {
                Console.WriteLine($"All players passed, round end.");
                shouldTerminateRound = true;
                break;
            }

            continue;
        }

        gameHandler.WriteGameState();

        shouldTerminate = !gameHandler.ConsoleActionHelper();

        gameHandler.EndTurn();

        var currentPower = gameHandler.Context.PlayFields[activePlayerId].GetOfType<FieldPowerMixin>().FirstOrDefault()
            ?.Value.ToString();


        Console.WriteLine($"\t Total Power on board for player {activePlayerId} is {currentPower} ");
    }

    shouldTerminate = !gameHandler.EndRound();
    gameHandler.WriteScore();
}