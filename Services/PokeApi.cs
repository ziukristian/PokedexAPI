using PokedexAPI.Interfaces;
using PokedexAPI.Models;
using System.Text.Json.Nodes;

namespace PokedexAPI.Services
{
    public class PokeApi : IPokeInfoService
    {
        private readonly HttpClient _client;

        public PokeApi()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };
        }
        public async Task<Pokemon> GetPokemonInfo(string pokemonName)
        {
            try
            {
                var response = await _client.GetAsync($"pokemon-species/{pokemonName}");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                JsonNode jsonObject = JsonNode.Parse(responseBody);

                Pokemon pokemon = MapResponseToPokemon(jsonObject);

                pokemon.Description = GetFlavorTextByLanguageCode(jsonObject);

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

        private static Pokemon MapResponseToPokemon(JsonNode jsonObject)
        {

            if(jsonObject == null)
            {
                throw new Exception("Response object cannot be mapped because NULL");
            }

            if (jsonObject["name"] == null || jsonObject["habitat"]?["name"] == null || jsonObject["is_legendary"] == null )
            {
                throw new Exception("Pokemon data not found in response object");
            }

            return new Pokemon
            {
                Name = jsonObject["name"].ToString(),
                Habitat = jsonObject["habitat"]["name"].ToString(),
                IsLegendary = jsonObject["is_legendary"].ToString() == "true"
            };
        }

        private static string GetFlavorTextByLanguageCode(JsonNode jsonObject, string languageCode = "en")
        {
            var entries = jsonObject["flavor_text_entries"]?.AsArray();

            if (entries == null)
            {
                throw new Exception("Pokemon description collection not found in response object");
            }

            var englishEntry = entries.FirstOrDefault(entry => entry?["language"]?["name"]?.ToString() == languageCode);

            if (englishEntry == null)
            {
                throw new Exception($"Description with language code '{languageCode}' not found in response object");
            }

            var description = englishEntry["flavor_text"]?.ToString();

            if (description == null)
            {
                throw new Exception("Pokemon description not found in response object");
            }

            return description;
        }
    }
}
