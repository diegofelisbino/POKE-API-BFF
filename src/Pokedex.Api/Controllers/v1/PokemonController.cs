using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Extensions;
using Pokedex.Api.ViewModels;
using Pokedex.Application.Interfaces;

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

        public PokemonController(IPokemonService service,
                                    IMapper mapper,
                                    INotificador notificador,
                                    ILogger<PokemonController> logger) : base(notificador)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

       /*[ [ClaimsAuthorize("Pokemon", "Listar")]     
        [HttpGet("{id:long}")]
        public async Task<ActionResult<PokemonDetailViewModel>> ObterPokemonPorId(long id)
        {
            _logger.LogInformation("Buscando Pokemon atavés do id {id}", id);

            var response = await _service.ObterPokemonPorId(id);

            var pokemonViewModel = _mapper.Map<PokemonDetailViewModel>(response);

            return CustomResponse(pokemonViewModel);

        }

        ClaimsAuthorize("Pokemon", "Listar")]
        [HttpGet]
        public async Task<ActionResult<PokemonListViewModel>> ObterTodosPokemons()
        {
            _logger.LogInformation("Buscando lista com todos Pokemons");

            var responseApi = await _service.ObterTodosPokemons();

            var pokemons = _mapper.Map<PokemonListViewModel>(responseApi);

            return CustomResponse(pokemons);
        }*/


       
    }
}
