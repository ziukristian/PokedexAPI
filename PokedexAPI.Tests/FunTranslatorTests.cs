using FluentAssertions;
using PokedexAPI.Models;
using PokedexAPI.Services;

namespace PokedexAPI.Tests
{
    public class FunTranslatorTests
    {

        private readonly FunTranslator _funTranslator;

        public FunTranslatorTests()
        {
            _funTranslator = new FunTranslator();
        }

        [Fact]
        public void GetLanguageByPokemon_ShouldReturnYoda_WhenPokemonIsLegendary()
        {
            // Arrange
            var pokemon = new Pokemon
            {
                Name = "Mewtwo",
                Description = "",
                Habitat = "",
                IsLegendary = true
            };

            // Act
            var language = _funTranslator.GetLanguageByPokemon(pokemon);

            // Assert
            language.Should().Be("yoda");
        }

        [Fact]
        public void GetLanguageByPokemon_ShouldReturnYoda_WhenPokemonHabitatIsCave()
        {
            // Arrange
            var pokemon = new Pokemon
            {
                Name = "Zubat",
                Description = "",
                Habitat = "cave",
                IsLegendary = false
            };

            // Act
            var language = _funTranslator.GetLanguageByPokemon(pokemon);

            // Assert
            language.Should().Be("yoda");
        }

        [Fact]
        public void GetLanguageByPokemon_ShouldReturnShakespeare_WhenPokemonIsNotLegendaryAndHabitatIsNotCave()
        {
            // Arrange
            var pokemon = new Pokemon
            {
                Name = "Lucario",
                Description = "",
                Habitat = "",
                IsLegendary = false
            };

            // Act
            var language = _funTranslator.GetLanguageByPokemon(pokemon);

            // Assert
            language.Should().Be("shakespeare");
        }
    }
}