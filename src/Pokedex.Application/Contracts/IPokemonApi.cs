using Pokedex.Application.Contracts.v1.Responses;
using Refit;

namespace Pokedex.Application.Contracts
{

    public interface IPokemonApi
    {
        [Get("/pokemon/{id}")]
        Task<ApiResponse<Pokemon>> ObterPokemonPorId(long id);

        [Get("/pokemon/?offset=0&limit=-1")]
        Task<ApiResponse<PokeList>> ObterTodosPokemons();


        [Get("/pokemon/?offset={offSet}&limit={limit}")]
        Task<PokeList> ObterPokemonsPaginado(int offSet, int limit);

        [Get("/pokemon/{nome}")]
        Task<Pokemon> ObterPokemonPorNome(string nome);        
    }
}
