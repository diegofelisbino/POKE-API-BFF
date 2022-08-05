using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Configurations;
using Pokedex.Api.ViewModels;
using Pokedex.Application.Interfaces;
using System.Net;

namespace Pokedex.Api.Controllers.v1
{
    [Route(ApiConfig.MINHA_ROTA_BASE_V1)]    
    public class PokemonController : MainController
    {
        private readonly IPokemonService _service;
        private readonly IMapper _mapper;
        public PokemonController(IPokemonService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;   
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<PokemonDetailViewModel>> ObterPokemonPorId(long id)
        {            
            var responseApi = await _service.ObterPokemonPorId(id);
            
            if (!responseApi.IsSuccessStatusCode) return BadRequest($"Falha ao buscar Pokemon no repositório ({responseApi.StatusCode})");

            var pokemon = _mapper.Map<PokemonDetailViewModel>(responseApi.Content);
            
            return Ok(pokemon);
        }

        [HttpGet]
        public async Task<ActionResult<PokemonListViewModel>> ObterTodosPokemons()
        {
            var responseApi = await _service.ObterTodosPokemons();

            if (!responseApi.IsSuccessStatusCode) return BadRequest($"Falha ao buscar Pokemon no repositório ({responseApi.StatusCode})");

            var pokemons = _mapper.Map<PokemonListViewModel>(responseApi.Content); 
            
            return Ok(pokemons);
        }


        /*[HttpGet("paginado")]
        public async Task<ActionResult<PokemonListPaginadoViewModel>> ObterPokemonsPaginado([FromQuery] int offset, int limit)
        {
            var pokemons = await _service.ObterPokemonsPaginado(offset, limit);

            return Ok(pokemons);
        }

        [HttpGet("{nome}")]
        public async Task<ActionResult<PokemonDetailViewModel>> ObterPokemonPorNome(string nome)
        {
            try
            {
                var pokemon = await _service.ObterPokemonPorNome(nome);

                return Ok(pokemon);
            }
            catch (Refit.ApiException ae)
            {
                if (ae.StatusCode != HttpStatusCode.OK)
                {
                    if (ae.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound("Dados não encontrados!");
                    }
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }*/
    }
}
