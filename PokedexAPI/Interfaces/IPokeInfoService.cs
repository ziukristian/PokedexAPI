using PokedexAPI.Models;

namespace PokedexAPI.Interfaces
{
    public interface IPokeInfoService
    {
        Task<Pokemon> GetPokemonInfo(string pokemonName);
    }
}
