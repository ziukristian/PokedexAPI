using Microsoft.AspNetCore.Http.HttpResults;
using PokedexAPI;

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

app.MapGet("/pokemon/translated/{pokemonName}", (string pokemonName) =>
{
    return Results.Ok(pokemonName);
})
.WithOpenApi();

app.Run();


