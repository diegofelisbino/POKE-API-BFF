
using AutoMapper;
using FluentAssertions;
using Moq;
using Pokedex.Application.Contracts;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Models;
using Pokedex.Application.Notificacoes;
using Pokedex.Application.Services;
using Pokedex.Domain.Interfaces;
using Pokedex.Domain.Services;
using Pokedex.Test.Mocks;
using Refit;
using System.Net;

namespace Pokedex.Test.UnitTests.PokemonServiceTest;

[CollectionDefinition(nameof(PokemonCollection))]
public class PokemonServiceTest : IClassFixture<PokemonTestsFixture>
{
    private readonly Mock<IPokemonApi> _pokemonApi;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IAspNetData> _aspNetData;
    private readonly Mock<INotificador> _notificador;

    private PokemonService _pokemonService;
    private readonly PokemonTestsFixture _pokemonTestsFixture;

    public PokemonServiceTest(PokemonTestsFixture pokemonTestsFixture)
    {
        _mapper = new Mock<IMapper>();
        _pokemonApi = new Mock<IPokemonApi>();
        _notificador = new Mock<INotificador>();
        _aspNetData = new Mock<IAspNetData>();
    
        _pokemonService = new PokemonService(_pokemonApi.Object,
                                             _mapper.Object,
                                             _notificador.Object,
                                             _aspNetData.Object);

        _pokemonTestsFixture = pokemonTestsFixture;
    }

    [Fact(DisplayName = "Retornar PokemonDetailModel")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_HttpStatusCodeOK_DeveRetornarPokemonDetailModel()
    {
        //Arrange
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), _pokemonTestsFixture.GerarPokemonResponseValido(), null);

        var pokemonDetailModel = _pokemonTestsFixture.GerarPokemonModelValido();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailModel);

        if (!response.IsSuccessStatusCode)
        {   
            _notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));            
            
            pokemonDetailModel = new PokemonDetailModel();
        }

        //Act            
        var resultadoObtido = await _pokemonService.ObterPokemonPorId(1);

        //Assert
        resultadoObtido.Should().NotBeNull();
        resultadoObtido.Should().BeEquivalentTo(pokemonDetailModel);
    }

    [Fact(DisplayName = "Retornar Null quando IsNotSuccess")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_ResponseNotSuccess_DeveRetornarNull()
    {
        //Arrange                
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);

        if (!response.IsSuccessStatusCode)
        {
            _notificador.Setup(x => x.Notificar(new Notificacao(It.IsAny<String>())));
        }

        //Act                
        var resultadoObtido = await _pokemonService.ObterPokemonPorId(1);

        //Assert
        resultadoObtido.Should().BeEquivalentTo(new PokemonDetailModel());

    }

    [Fact(DisplayName = "Retornar PokemonDetailModel sem NiveisDePoder")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_StatsSemElementos_NiveisDePoderNaoDeveRetornarElementos()
    {
        //Arrange        
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), _pokemonTestsFixture.GerarPokemonResponseSemStats(), null);

        var pokemonDetailModel = _pokemonTestsFixture.GerarPokemonModelValido();
        pokemonDetailModel.NiveisDePoder = new Dictionary<string, long>();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailModel);

        if (response.Content.Stats.Any())
        {
            pokemonDetailModel.NiveisDePoder = _pokemonTestsFixture.GerarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var resultadoObtido = await _pokemonService.ObterPokemonPorId(1);

        //Assert         
        Assert.True(resultadoObtido.NiveisDePoder.Count == 0);
    }

    [Fact(DisplayName = "Retornar NiveisDePoder baseado na lista de Stats")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_StatsRetornarElementos_NiveisDePoderDeveRetornarElementos()
    {
        //Arrange        
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), _pokemonTestsFixture.GerarPokemonResponseValido(), null, null);

        var pokemonDetailModelFaker = _pokemonTestsFixture.GerarPokemonModelValido();
        pokemonDetailModelFaker.NiveisDePoder = new Dictionary<string, long>();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailModelFaker);

        if (response.Content.Stats.Any())
        {
            pokemonDetailModelFaker.NiveisDePoder = _pokemonTestsFixture.GerarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var pokemonDetailModel = await _pokemonService.ObterPokemonPorId(1);

        //Assert        
        Assert.Equal(pokemonDetailModelFaker.NiveisDePoder.Count, pokemonDetailModel.NiveisDePoder.Count);

    }

    [Fact(DisplayName = "Não retonar MaxStats, quando Stats retornar somente 1 elemento")]
    [Trait("Categoria", "RecuperarNiveisDePoderPorStats")]
    public void RecuperarNiveisDePoderPorStats_ReceberUmElemento_NaoDeveRetornarMaxStat()
    {
        //Arrange
        var content = _pokemonTestsFixture.GerarPokemonResponseComUmStats();

        //Act
        var niveisPoder = _pokemonService.RecuperarNiveisDePoderPorStats(content.Stats);

        //Assert        
        Assert.True(!niveisPoder?.Keys.Contains("max-stat"));
    }

    [Fact(DisplayName = "O MaxBaseStat deve ter o valor do maior Stats")]
    [Trait("Categoria", "RecuperarNiveisDePoderPorStats")]
    public void RecuperarNiveisDePoderPorStats_ValorDoMaxBaseStat_DeveTerMaiorValorStats()
    {
        //Arrange
        var content = _pokemonTestsFixture.GerarPokemonResponseValido();

        long resultadoEsperado = content.Stats.Select(s => s.BaseStat).Max();

        //Act
        var niveisPoder = _pokemonService.RecuperarNiveisDePoderPorStats(content.Stats);

        long resultadoObtido = niveisPoder.Where(x => x.Key == "max-stat").Select(x => x.Value).First();

        //Assert
        Assert.Equal(resultadoEsperado, resultadoObtido);
    }

    [Fact(DisplayName = "Retornar PokemonListModel")]
    [Trait("Categoria", "ObterTodosPokemons")]
    public async Task ObterTodosPokemons_ResponseSucesso_DeveRetornarPokemonListModel()
    {
        //Arrange
        var pokeList = _pokemonTestsFixture.GerarPokeListValido(10);

        var pokeListModel = _pokemonTestsFixture.GerarPokemonListModel(pokeList);

        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.OK), pokeList, null);

        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);
        _mapper.Setup(x => x.Map<PokemonListModel>(response.Content)).Returns(pokeListModel);

        if (!response.IsSuccessStatusCode)
        {
            _notificador.Setup(x => x.Notificar(new Notificacao(It.IsAny<String>())));
        }

        //Act
        var resultadoObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        resultadoObtido.Should().NotBeNull();
        resultadoObtido.Should().BeEquivalentTo(pokeListModel);
    }

    [Fact(DisplayName = "Retornar Null quando IsNotSuccess")]
    [Trait("Categoria", "ObterTodosPokemons")]
    public async Task ObterTodosPokemon_Quando_Response_Not_Success()
    {
        //Arrange                
        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);

        if (!response.IsSuccessStatusCode)
        {
            _notificador.Setup(x => x.Notificar(new Notificacao(It.IsAny<String>())));
        }

        //Act                
        var pokemonObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        pokemonObtido.Should().BeEquivalentTo(new PokemonListModel());
    }



}

