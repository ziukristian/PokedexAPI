using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using PokedexAPI.Interfaces;
using PokedexAPI.Models;
using PokedexAPI.Services;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services
builder.Services.AddSingleton<IPokeInfoService, PokeApi>();
builder.Services.AddSingleton<ITranslatorService, FunTranslator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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






