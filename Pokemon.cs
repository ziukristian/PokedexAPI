using System.Text.Json.Serialization;

namespace PokedexAPI
{
    public class Pokemon
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [JsonPropertyName("test")]
        public string Habitat { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

    }
}
