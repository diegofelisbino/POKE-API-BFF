
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;
using Refit;

namespace Pokedex.Application.Interfaces
{
    public interface IPokemonService
    {
        /*Task<PokemonDetailViewModel> ObterPokemonPorId(long id);
        Task<PokemonDetailViewModel> ObterPokemonPorNome(string nome);
        Task<PokemonListPaginadoViewModel> ObterPokemonsPaginado(int offset, int limit);
        Task<PokemonListViewModel> ObterTodosPokemons();*/

        Task<ApiResponse<PokemonDetailModel>> ObterPokemonPorId(long id);
        Task<ApiResponse<PokemonListModel>> ObterTodosPokemons();

        /*Task<PokemonListPaginadoModel> ObterPokemonsPaginado(int offset, int limit);
        Task<PokemonDetailModel> ObterPokemonPorNome(string nome);*/

    }
}
