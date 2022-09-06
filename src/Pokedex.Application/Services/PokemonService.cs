using AutoMapper;
using Pokedex.Application.Configurations;
using Pokedex.Application.Contracts;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Models;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Services;


namespace Pokedex.Application.Services;

public class PokemonService : BaseService, IPokemonService
{
    private readonly IPokemonApi _pokemonApi;
    private readonly IMapper _mapper;
    private readonly IAspNetData _aspNetData;    
    public PokemonService(IPokemonApi pokemonApi, 
                          IMapper mapper, 
                          INotificador notificador, 
                          IAspNetData aspNetData) : base (notificador)
    {
        //_pokemonApi = RestService.For<IPokemonApi>(Configurations.Config.BASE_ADRESS_EXTERNAL_API);
        _pokemonApi = pokemonApi;
        _mapper = mapper;
        _aspNetData = aspNetData;
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

        if (response.Content.Stats.Any())
        {
            pokemonDetail.NiveisDePoder = RecuperarNiveisDePoderPorStats(response.Content.Stats);
        }

        return pokemonDetail;
    }

    public Dictionary<string, long> RecuperarNiveisDePoderPorStats(List<StatX> stats)
    {
        var niveisDePoder = new Dictionary<string, long>();

        if (stats.Any())
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

        pokemonList.Pokemons = ObterPokemons(pokemonList);

        return pokemonList;

    }
    
   
    public List<PokemonBaseModel> ObterPokemons(PokemonListModel pokemonList)
    {
        List<PokemonBaseModel> lista = new();

        if (pokemonList.Results.Any())
        {
            foreach (var result in pokemonList.Results)
            {
                
                PokemonBaseModel pokemonBase = new PokemonBaseModel();
                pokemonBase.Id = long.Parse(result.Url.Segments[4].ToString());                
                pokemonBase.Nome = result.Name;
                pokemonBase.ImagemUrl = new Uri($"{Config.BASE_ADRESS_IMAGEM}{pokemonBase.Id}.png");
                pokemonBase.Url = new Uri($"{_aspNetData.AddressObterPokemonPorId}{pokemonBase.Id}");

                lista.Add(pokemonBase);
            }
        }

        return lista;
    }



}

