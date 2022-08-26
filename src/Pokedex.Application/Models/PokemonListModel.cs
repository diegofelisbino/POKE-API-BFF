

using Pokedex.Application.Contracts.v1.Responses;

namespace Pokedex.Application.Models
{
    public class PokemonListModel
    {
        public List<Results> Results { get; set; }        
        public List<PokemonBaseModel> Pokemons { get; set; }
    }
}
