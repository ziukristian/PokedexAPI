using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;
using PokedexAPI.Interfaces;
using PokedexAPI.Models;

namespace PokedexAPI.Services
{
    public class FunTranslator : ITranslatorService
    {
        private readonly HttpClient _client;

        public FunTranslator()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.funtranslations.com/")
            };
        }

        public async Task<string> Translate(string text, string language)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { text }), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync($"translate/{language}.json", content);
                var responseBody = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();

                JsonNode jsonObject = JsonNode.Parse(responseBody);

                var translatedTextNode = jsonObject?["contents"]?["translated"];

                //Check if the response contains the translated text
                if (translatedTextNode == null)
                {
                    throw new Exception();
                }

                return translatedTextNode.ToString();
            }
            catch
            {
                // If the translation fails (for any reason!) , return the original text
                return text;
            }
        }

        public string GetLanguageByPokemon(Pokemon pokemon)
        {
            // If the pokemon is legendary or lives in a cave, return Yoda translator. If not it's Shakespeare
            return pokemon.IsLegendary || pokemon.Habitat == "cave" ? "yoda" : "shakespeare";
        }
    }
}
