using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;

namespace Pokedex.Test.Mocks
{
    public static class PokemonListModelMock
    {
        public static PokemonListModel ConverterPokeListEmPokeListModel(PokeList pokeList)
        {
            var pokemons = new List<PokemonBaseModel>();

            foreach (var result in pokeList.Results)
            {
                PokemonBaseModel pokemonBase = new PokemonBaseModel();
                pokemonBase.Id = long.Parse(result.Url.Segments[4].ToString().Replace("/", ""));
                pokemonBase.Nome = result.Name;
                pokemons.Add(pokemonBase);
            }

            var pokemonListModel = new  PokemonListModel();
            pokemonListModel.Results = pokeList.Results;
            //pokemonListModel.Pokemons = pokemons;          

            return pokemonListModel;    
        }
    }
}
