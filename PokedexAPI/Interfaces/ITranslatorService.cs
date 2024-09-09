using PokedexAPI.Models;

namespace PokedexAPI.Interfaces
{
    public interface ITranslatorService
    {
        Task<string> Translate(string text, string language);
        string GetLanguageByPokemon(Pokemon pokemon);
    }
}
