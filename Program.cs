using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using PokedexAPI;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pokemon/{pokemonName}", (string pokemonName) =>
{
    return Results.Ok(pokemonName);
})
.WithOpenApi();

app.MapGet("/pokemon/translated/{pokemonName}", async (string pokemonName) =>
{
    var pokemon = await GetPokemonInfo(pokemonName);
    var translation = await Translate(pokemon.Description, "shakespeare");

    pokemon.Description = translation;

    return Results.Ok(pokemon);
})
.WithOpenApi();

app.Run();

static async Task<Pokemon> GetPokemonInfo(string pokemonName)
{
    try
    {
        HttpClient client = new();
        var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon-species/{pokemonName}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        JsonNode jsonObject = JsonNode.Parse(responseBody);

        var pokemon = new Pokemon
        {
            Name = jsonObject["name"]?.ToString(),
            Habitat = jsonObject["habitat"]?["name"]?.ToString(),
            IsLegendary = jsonObject["is_legendary"] != null && jsonObject["is_legendary"]?.ToString() == "true"
        };

        var englishFlavorText = jsonObject["flavor_text_entries"]?
            .AsArray()
            .FirstOrDefault(entry => entry["language"]?["name"]?.ToString() == "en")?["flavor_text"]?.ToString();

        pokemon.Description = englishFlavorText;

        return pokemon;
    }
    catch (HttpRequestException)
    {
        throw new Exception("Pokemon data could not be retrieved");
    }
    catch (Exception)
    {
        throw;
    }
}

static async Task<string> Translate(string text, string language)
{
    try
    {
        using HttpClient client = new();

        var encodedText = HttpUtility.UrlEncode(text);

        // Does not work without, for some reason
        StringContent content = new StringContent("{}", Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"https://api.funtranslations.com/translate/{language}.json?text={encodedText}", content);

        var responseBody = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        JsonNode jsonObject = JsonNode.Parse(responseBody);

        return jsonObject["contents"]?["translated"]?.ToString();
    }
    catch (HttpRequestException)
    {
        throw new Exception("Translation could not be retrieved");
    }
    catch (Exception)
    {
        throw;
    }
}





