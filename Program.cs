using System.Text.Json.Serialization;

var app = WebApplication.Create(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

app.MapGet("/", () => "Let the battle begin!");
app.MapPost("/", (ArenaUpdate model) =>
{
    Console.WriteLine(model);
    string[] commands = { "F", "R", "L", "T" };
    var i = Random.Shared.Next(4);

    return commands[i];
});

app.Run($"http://0.0.0.0:{port}");


internal record ArenaUpdate([property: JsonPropertyName("_links")] Links Links, Arena Arena);

internal record Links(Self Self);

internal record Self(string Href);

internal record Arena(List<int> Dims, Dictionary<string, PlayerState> State);

internal record PlayerState(int X, int Y, string Direction, bool WasHit, int Score);