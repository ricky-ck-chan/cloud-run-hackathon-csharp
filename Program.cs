using System.Text.Json.Serialization;

var app = WebApplication.Create(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

app.MapGet("/", () => "Let the battle begin!");
app.MapPost("/", (ArenaUpdate model) =>
{
    var result = Flow.Process(model);
    Visual.AddMessageLine($"Result [{result}]");
    Visual.PrintMessage();
    return result;
});

app.Run($"http://0.0.0.0:{port}");


internal record ArenaUpdate([property: JsonPropertyName("_links")] Links Links, Arena Arena);

internal record Links(Self Self);

internal record Self(string Href);

internal record Arena(List<int> Dims, Dictionary<string, PlayerState> State);

internal record PlayerState(int X, int Y, string Direction, bool WasHit, int Score);