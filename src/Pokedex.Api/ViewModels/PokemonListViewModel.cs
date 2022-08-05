
namespace Pokedex.Api.ViewModels
{
    public record PokemonListViewModel
    {  
        public List<PokemonBaseViewModel> Pokemons { get; set; }
    }
}