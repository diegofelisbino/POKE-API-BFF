
namespace Pokedex.Api.ViewModels
{
    public record PokemonListPaginadoViewModel : PokemonListViewModel
    {
        public int Quantidade { get; set; }
        public Uri Proximo { get; set; }
        public Uri Anterior { get; set; }

    }
}

   


    

