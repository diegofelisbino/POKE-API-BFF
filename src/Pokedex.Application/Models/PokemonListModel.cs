

using Pokedex.Application.Contracts.v1.Responses;

namespace Pokedex.Application.Models
{
    public class PokemonListModel
    {
        public List<Results> Results { get; set; }

        private List<PokemonBaseModel> _pokemons;
        public List<PokemonBaseModel> Pokemons
        {
            get
            {
                _pokemons = new List<PokemonBaseModel>();

                if (this.Results != null && this.Results.Count >= 1)
                {
                    foreach (var result in this.Results)
                    {
                        PokemonBaseModel pokemonBase = new PokemonBaseModel();
                        pokemonBase.Id = long.Parse(result.Url.Segments[4].ToString().Replace("/", ""));
                        pokemonBase.Nome = result.Name;
                        _pokemons.Add(pokemonBase);
                    }
                }
                return _pokemons;
            }
            //set { _pokemons = value; }            

        }

    }
}
