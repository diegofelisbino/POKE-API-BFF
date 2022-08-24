using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Contracts;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Models;
using Pokedex.Application.Notificacoes;
using Pokedex.Domain.Services;

namespace Pokedex.Application.Services;

public class PokemonService : BaseService, IPokemonService
{
    private readonly IPokemonApi _pokemonApi;
    private readonly IMapper _mapper;


    public PokemonService(IPokemonApi pokemonApi, IMapper mapper, INotificador notificador) : base(notificador)
    {
        //_pokemonApi = RestService.For<IPokemonApi>(Configurations.Config.BASE_ADRESS_EXTERNAL_API);
        _pokemonApi = pokemonApi;
        _mapper = mapper;
    }

    public async Task<PokemonDetailModel> ObterPokemonPorId(long id)
    {
       
        var pokemonDetail = new PokemonDetailModel();

        var response = await _pokemonApi.ObterPokemonPorId(id);

        if (!response.IsSuccessStatusCode)
        {
            base.Notificar("Falha na comunicação com a Api Principal");
            return pokemonDetail;
        }

        pokemonDetail = _mapper.Map<PokemonDetailModel>(response.Content);

        pokemonDetail.NiveisDePoder = new Dictionary<string, long>();

        if (response.Content.Stats.Count >= 1)
        {
            pokemonDetail.NiveisDePoder = RecuperarNiveisDePoderPorStats(response.Content.Stats);
        }

        return pokemonDetail;

    }

    public async Task<PokemonListModel> ObterTodosPokemons()
    {
        var pokemonList = new PokemonListModel();

        var response = await _pokemonApi.ObterTodosPokemons();

        if (!response.IsSuccessStatusCode)
        {
            base.Notificar("Falha na comunicação com a Api Principal");
            return pokemonList;
        }

        pokemonList = _mapper.Map<PokemonListModel>(response.Content);
        return pokemonList;



    }

    /*public async Task<PokemonListPaginadoModel> ObterPokemonsPaginado(int offset, int limit)
    {
        ApiConfig.OffSet = offset;
        ApiConfig.Limit = limit;

        return _mapper.Map<PokemonListPaginadoModel>(_mapper.Map<PokemonListPaginadoModel>(await _pokemonApi.ObterPokemonsPaginado(ApiConfig.OffSet, ApiConfig.Limit)));
    }

    public async Task<PokemonDetailModel> ObterPokemonPorNome(string nome)
    {
        return _mapper.Map<PokemonDetailModel>(_mapper.Map<PokemonDetailModel>(await _pokemonApi.ObterPokemonPorNome(nome)));
    }
     */

    public Dictionary<string, long> RecuperarNiveisDePoderPorStats(List<StatX> stats)
    {
        var niveisDePoder = new Dictionary<string, long>();

        if (stats.Count >= 1)
        {
            foreach (var stat in stats)
            {
                niveisDePoder.Add(stat.Stat.Name, stat.BaseStat);
            }
        }
        if (stats.Count > 1)
        {
            string maxStat = "max-stat";
            long maxBaseStat = stats.Select(s => s.BaseStat).Max();
            niveisDePoder.Add(maxStat, maxBaseStat);

        }
        return niveisDePoder;
    }

}

