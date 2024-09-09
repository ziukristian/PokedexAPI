using FluentAssertions;
using PokedexAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PokedexAPI.Tests
{
    public class PokeApiTests
    {
        private readonly PokeApi _pokeApi;

        public PokeApiTests()
        {
            _pokeApi = new PokeApi();
        }

        [Fact]
        public async Task MapResponseToPokemon_ShouldThrow_WhenJsonIsNull()
        {
            // Arrange
            JsonNode jsonObject = null;

            // Act
            Action act = () => PokeApi.MapResponseToPokemon(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task MapResponseToPokemon_ShouldThrow_WhenNameIsMissing()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"habitat\": {\"name\": \"forest\"}, \"is_legendary\": false}");

            // Act
            Action act = () => PokeApi.MapResponseToPokemon(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task MapResponseToPokemon_ShouldThrow_WhenHabitatIsMissing()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"name\": \"bulbasaur\", \"is_legendary\": false}");

            // Act
            Action act = () => PokeApi.MapResponseToPokemon(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task MapResponseToPokemon_ShouldThrow_WhenHabitatNameIsMissing()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"name\": \"bulbasaur\", \"habitat\": {}, \"is_legendary\": false}");

            // Act
            Action act = () => PokeApi.MapResponseToPokemon(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task MapResponseToPokemon_ShouldThrow_WhenIsLegendaryIsNull()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"name\": \"bulbasaur\", \"habitat\": {\"name\": \"forest\"}}");

            // Act
            Action act = () => PokeApi.MapResponseToPokemon(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task MapResponseToPokemon_ShouldReturnPokemon_WhenEverthingIsPresent()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"name\": \"bulbasaur\", \"habitat\": {\"name\": \"forest\"}, \"is_legendary\": false}");

            // Act
            var pokemon = PokeApi.MapResponseToPokemon(jsonObject);

            // Assert
            pokemon.Name.Should().Be("bulbasaur");
            pokemon.Habitat.Should().Be("forest");
            pokemon.IsLegendary.Should().BeFalse();
        }

        [Fact]
        public async Task GetFlavorTextByLanguageCode_ShouldThrow_WhenJsonIsNull()
        {
            // Arrange
            JsonNode jsonObject = null;

            // Act
            Action act = () => PokeApi.GetFlavorTextByLanguageCode(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task GetFlavorTextByLanguageCode_ShouldThrow_WhenEntriesArrayIsMissing()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{}");

            // Act
            Action act = () => PokeApi.GetFlavorTextByLanguageCode(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task GetFlavorTextByLanguageCode_ShouldThrow_WhenRequiredLanguageIsMissing()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"flavor_text_entries\": [{\"flavor_text\": \"What is better? To be born good? Or to overcome your evil nature by great effort?\", \"language\": {\"name\": \"fr\"}}]}");

            // Act
            Action act = () => PokeApi.GetFlavorTextByLanguageCode(jsonObject, "it");

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task GetFlavorTextByLanguageCode_ShouldThrow_WhenDescriptionIsMissing()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"flavor_text_entries\": [{\"language\": {\"name\": \"en\"}}]}");

            // Act
            Action act = () => PokeApi.GetFlavorTextByLanguageCode(jsonObject);

            // Assert
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task GetFlavorTextByLanguageCode_ShouldReturnFlavorText_WhenEverythingIsPresent()
        {
            // Arrange
            JsonNode jsonObject = JsonNode.Parse("{\"flavor_text_entries\": [{\"flavor_text\": \"What is better? To be born good? Or to overcome your evil nature by great effort?\", \"language\": {\"name\": \"en\"}}]}");

            // Act
            var flavorText = PokeApi.GetFlavorTextByLanguageCode(jsonObject);

            // Assert
            flavorText.Should().Be("What is better? To be born good? Or to overcome your evil nature by great effort?");
        }

    }
}
