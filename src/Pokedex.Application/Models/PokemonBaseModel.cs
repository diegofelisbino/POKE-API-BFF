
using Pokedex.Application.Configurations;
using Pokedex.Domain.Interfaces;

namespace Pokedex.Application.Models
{
    public class PokemonBaseModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public Uri ImagemUrl { get; set; }
        public Uri Url { get; set; }

    }
}
