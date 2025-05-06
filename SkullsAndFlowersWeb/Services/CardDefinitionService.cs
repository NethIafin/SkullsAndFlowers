using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace SkullsAndFlowersWeb.Services;

public sealed class CardDefinitionService : IDisposable
{
    private readonly IWebHostEnvironment      _env;
    private readonly NavigationManager        _nav;
    private readonly Dictionary<string, Dictionary<string, CardDefinition>> _cache = new();
    private Dictionary<string, CardDefinition> _cardDefinitions = new();
    private string _lang = "en";                       // default language code

    public string CurrentLanguage => _lang;

    public CardDefinitionService(IWebHostEnvironment env, NavigationManager nav)
    {
        _env = env;
        _nav = nav;

        UpdateLanguageFromUri(_nav.Uri, reload: true);
        _nav.LocationChanged += HandleLocationChanged;
    }

    /* ---------- public API ---------- */
    public CardDefinition GetCardDefinition(string cardId)
    {
        if (cardId != null && _cardDefinitions.TryGetValue(cardId.ToLower(), out var def))
            return def;

        return new()     // minimal fallback
        {
            Description = "No description available",
            Keywords    = Array.Empty<string>(),
            ImageUrl    = "images/cards/default.png",
            RarityColor = "#6c757d"
        };
    }

    /* ---------- helpers ---------- */
    private void HandleLocationChanged(object? _, LocationChangedEventArgs args)
        => UpdateLanguageFromUri(args.Location);

    private void UpdateLanguageFromUri(string uri, bool reload = false)
    {
        var query = QueryHelpers.ParseQuery(new Uri(uri).Query);
        var requested = query.TryGetValue("lang", out var l) ? l.ToString().ToLower() : "en";

        if (requested == _lang && !reload) return;   // no change

        _lang = requested;
        _cardDefinitions = LoadDefinitionsFor(_lang);
    }

    private Dictionary<string, CardDefinition> LoadDefinitionsFor(string lang)
    {
        if (_cache.TryGetValue(lang, out var cached))
            return cached;

        // Determine file name: card-definitions-LANG.json   or base file for default/en
        var fileName = lang is "en" or "" ? "card-definitions.json"
                                          : $"card-definitions-{lang}.json";

        var path = Path.Combine(_env.WebRootPath, "data", fileName);

        // Fallback chain: requested file -> base file -> empty
        var jsonPath = File.Exists(path)
                        ? path
                        : Path.Combine(_env.WebRootPath, "data", "card-definitions.json");

        try
        {
            if (File.Exists(jsonPath))
            {
                var json  = File.ReadAllText(jsonPath);
                var defs  = JsonSerializer.Deserialize<Dictionary<string, CardDefinition>>(json,
                               new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                           ?? new();

                _cache[lang] = defs;                 // cache for later
                return defs;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading '{fileName}': {ex.Message}");
        }

        return new();                                 // last‑ditch empty dictionary
    }

    public void Dispose() => _nav.LocationChanged -= HandleLocationChanged;

    /* ---------- DTO ---------- */
    public class CardDefinition
    {
        public string Description { get; set; } = string.Empty;
        public string[] Keywords { get; set; } = Array.Empty<string>();
        public string ImageUrl { get; set; } = string.Empty;
        public string RarityColor { get; set; } = "#6c757d";
        public int? Power { get; set; }
        public string GlobalSymbol { get; set; } = string.Empty;
        public string? CustomName { get; set; } = null;
    }
}