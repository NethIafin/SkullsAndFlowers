using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SkullsAndFlowersWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register game services
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<DeckService>();
builder.Services.AddScoped<CardDefinitionService>();

builder.Services.AddDistributedMemoryCache();
// Add session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add this line to listen on all network interfaces
builder.WebHost.UseUrls("http://0.0.0.0:5073");

var app = builder.Build();

// Initialize sample decks on startup
var deckService = app.Services.GetRequiredService<DeckService>();
deckService.InitializeSampleDecks();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseSession();

app.MapRazorComponents<SkullsAndFlowersWeb.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();