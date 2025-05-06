using System.Collections.Concurrent;
using System.Text;
using SkullsAndFlowersGame;
using SkullsAndFlowersGame.CardSystem;
using SkullsAndFlowersGame.CardSystem.Mixins;
using Microsoft.Extensions.Hosting;

namespace SkullsAndFlowersWeb.Services;

/// <summary>
/// Stores decks in‑memory *and* persists them to a simple
/// text‑file “database”.
/// </summary>
public sealed class DeckService
{
    private readonly ConcurrentDictionary<string, Dictionary<string,int>> _savedDecks = new();

    private readonly string _dbFilePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);

    public DeckService(IHostEnvironment env)
    {
        var dataDir = Path.Combine(env.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDir);
        _dbFilePath = Path.Combine(dataDir, "decks.db");

        LoadDatabase();
    }

    public IEnumerable<string> GetAvailableCardNames()
    {
        GameStateHelper.InitCards();

        var cardTemplates = GameStateHelper
            .FindAndInstantiateImplementations<ICardTemplate>(typeof(ICardTemplate).Assembly);

        return cardTemplates
            .Select(t => t.GenerateCard())
            .Where(c => c.Identifier is not null &&
                        !c.GetOfType<IsTokenMixin>().Any())
            .Select(c => c.Identifier!)!;
    }

    public void SaveDeck(string deckName, params string[] cardSelections)
    {
        var cardDict = cardSelections
            .Select(s => s.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(a => (Card: a[0], Qty: int.Parse(a[1])))
            .Where(t => t.Qty > 0)   
            .ToDictionary(a => a.Card, a => a.Qty);

        SaveDeck(deckName, cardDict);
    }

    public void SaveDeck(string deckName, Dictionary<string,int> cardSelections)
    {
        _savedDecks[deckName] = cardSelections;
        PersistDatabase();               // flush to disk
    }

    public bool TryGetDeck(string deckName, out IDeck? deck)
    {
        deck = null;
        if (!_savedDecks.TryGetValue(deckName, out var cards))
            return false;

        deck = CreateDeck(cards);
        return true;
    }

    public IEnumerable<string> GetSavedDeckNames() => _savedDecks.Keys;

    public void InitializeSampleDecks()
    {
        if (_savedDecks.Count > 0) return;   // don’t pollute existing data

        SaveDeck("Sample Deck 1", "tulip:3", "daisy:3", "rose:3", "clear skies:3", "oak:3");
        SaveDeck("Sample Deck 2", "tulip:3", "daisy:3", "rose:3", "clear skies:3", "oak:3");
        SaveDeck("Debugging Deck", "forget-me-not weed:4", "buddy bud tree:4",
                 "birch:4", "dandelion:4", "monsoon:4");
    }

    private static IDeck CreateDeck(Dictionary<string,int> cardSelections)
    {
        var deckParams = cardSelections
            .Where(kvp => kvp.Value > 0)
            .Select(kvp => $"{kvp.Key}:{kvp.Value}")
            .ToArray();

        return GameStateHelper.GenerateDeck(deckParams);
    }

    /// <summary>Reads the file, if present, into <see cref="_savedDecks"/>.</summary>
    private void LoadDatabase()
    {
        if (!File.Exists(_dbFilePath)) return;

        string[] lines = File.ReadAllLines(_dbFilePath);
        for (int i = 0; i + 1 < lines.Length; i += 2)
        {
            string name  = lines[i].Trim();
            string deck  = lines[i + 1];

            var dict = deck.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                           .Select(s => s.Split(':', 2, StringSplitOptions.TrimEntries))
                           .ToDictionary(a => a[0], a => int.Parse(a[1]));

            _savedDecks[name] = dict;
        }
    }

    /// <summary>Writes the entire dictionary to disk atomically.</summary>
    private void PersistDatabase()
    {
        _fileLock.Wait();
        try
        {
            var sb = new StringBuilder();
            foreach (var (name, cards) in _savedDecks.OrderBy(k => k.Key))
            {
                sb.AppendLine(name);
                sb.AppendLine(string.Join(", ", cards.Where(x=>x.Value>0).Select(kvp => $"{kvp.Key}:{kvp.Value}")));
            }

            // Write to a temp file first, then replace – avoids partial writes
            string tmp = _dbFilePath + ".tmp";
            File.WriteAllText(tmp, sb.ToString());
            File.Move(tmp, _dbFilePath, overwrite: true);
        }
        finally
        {
            _fileLock.Release();
        }
    }
}
