using AutoMapper;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Configurations;
using Pokedex.Api.ViewModels;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Models;

namespace Pokedex.Api.Controllers.v1
{

    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pokemon")]
    public class PokemonController : MainController
    {
        private readonly IPokemonService _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PokemonController(   IPokemonService service, 
                                    IMapper mapper, 
                                    INotificador notificador, 
                                    ILogger<PokemonController> logger) : base(notificador)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("{id:long}")]
        public async Task<ActionResult<PokemonDetailViewModel>> ObterPokemonPorId(long id)
        {
            try
            {
                _logger.LogInformation("Buscando Pokemon atavés do id {id}", id);

                var response = await _service.ObterPokemonPorId(id);

                var pokemonViewModel = _mapper.Map<PokemonDetailViewModel>(response);
                
                return CustomResponse(pokemonViewModel);
            }
            catch (Exception ex)
            {   
                _logger.LogCritical(ex, "enpoint GET {endpoint} -> ObterPokemonPorId({id}) - Exception", Request.Path.Value, id );
                return CustomResponse();
                
            }
            
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PokemonListViewModel>> ObterTodosPokemons()
        {
            try
            {
                _logger.LogInformation("Buscando lista com todos Pokemons");

                var responseApi = await _service.ObterTodosPokemons();

                var pokemons = _mapper.Map<PokemonListViewModel>(responseApi);

                string url = $"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}";

                foreach (var pokemon in pokemons.Pokemons)
                {
                    pokemon.Url = new Uri($"{url}{pokemon.Id}");
                }

                return CustomResponse(pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "enpoint GET {endpoint} -> ObterTodosPokemons() - Exception", Request.Path.Value);
                return CustomResponse();

            }

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
