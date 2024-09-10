using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using PokedexAPI.Interfaces;
using PokedexAPI.Models;
using PokedexAPI.Services;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add custom services
builder.Services.AddSingleton<IPokeInfoService, PokeApi>();
builder.Services.AddSingleton<ITranslatorService, FunTranslator>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// ROUTES

// GET /pokemon/{pokemonName}
app.MapGet("/pokemon/{pokemonName}", async (IPokeInfoService pokeInfoService, string pokemonName) =>
{
    try
    {
        var pokemon = await pokeInfoService.GetPokemonInfo(pokemonName);

        return Results.Ok(pokemon);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
    
})
.WithOpenApi();


// GET /pokemon/translated/{pokemonName}
app.MapGet("/pokemon/translated/{pokemonName}", async (IPokeInfoService pokeInfoService, ITranslatorService translator, string pokemonName) =>
{
    try
    {
        var pokemon = await pokeInfoService.GetPokemonInfo(pokemonName);

        var translationLanguage = translator.GetLanguageByPokemon(pokemon); 
        var translation = await translator.Translate(pokemon.Description, translationLanguage);

        pokemon.Description = translation;

        return Results.Ok(pokemon);
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }
    
})
.WithOpenApi();

app.Run();






