
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;
using Pokedex.Application.Notificacoes;
using Refit;

namespace Pokedex.Application.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonDetailModel> ObterPokemonPorId(long id);
        Task<PokemonListModel> ObterTodosPokemons();
    }
}
