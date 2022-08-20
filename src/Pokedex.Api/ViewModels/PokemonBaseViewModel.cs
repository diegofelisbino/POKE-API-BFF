using Pokedex.Api.Configurations;

namespace Pokedex.Api.ViewModels
{
    public record PokemonBaseViewModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public Uri ImagemUrl { get; set; }      
        public Uri Url { get; set; }

    }
}






