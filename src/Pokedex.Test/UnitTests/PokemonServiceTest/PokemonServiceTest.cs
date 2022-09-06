
using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
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
    private readonly PokemonTestsFixture _pokemonTestsFixture;
    private readonly PokemonService _pokemonService;
    const string MENSAGEM_METODO_NOTIFICAR = "Falha na comunicação com a Api Principal";
    const string POKEMON_BASE_URL = "https://pokeapi.co/api/v2/pokemon/";
    public PokemonServiceTest(PokemonTestsFixture pokemonTestsFixture)
    {
        _pokemonTestsFixture = pokemonTestsFixture;
        _pokemonService = _pokemonTestsFixture.ObterPokemonService();

    }


    [Fact(DisplayName = "Notificador com mensagem empty")]
    [Trait("Categoria", "BaseService")]
    public void ObterPokemonPorId_NotificarMensagemStringEmpty_DeveChamarNotificador()
    {
        //Arrange
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);

        //Act
        if (!response.IsSuccessStatusCode)
        {
            _pokemonTestsFixture.Notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));
            _pokemonService.Notificar("");
        }

        //Assert        
        _pokemonTestsFixture.Notificador.Verify(x => x.Notificar(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact(DisplayName = "Retornar PokemonDetailModel")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_HttpStatusCodeOK_DeveRetornarPokemonDetailModel()
    {
        //Arrange
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), _pokemonTestsFixture.GerarPokemonResponseValido(), null);

        var pokemonDetailModel = _pokemonTestsFixture.GerarPokemonModelValido();

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);

        _pokemonTestsFixture.Mapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailModel);

        if (!response.IsSuccessStatusCode)
        {
            _pokemonTestsFixture.Notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));

            pokemonDetailModel = new PokemonDetailModel();
        }

        if (response.Content.Stats.Any())
        {
            pokemonDetailModel.NiveisDePoder = _pokemonTestsFixture.GerarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act            
        var resultadoObtido = await _pokemonService.ObterPokemonPorId(1);

        //Assert
        resultadoObtido.Should().NotBeNull();
        resultadoObtido.Should().BeEquivalentTo(pokemonDetailModel);
        resultadoObtido.NiveisDePoder.Should().HaveCount(response.Content.Stats.Count + 1);
    }

    [Fact(DisplayName = "Retornar PokemonDetailModel sem NiveisDePoder")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_StatsSemElementos_NiveisDePoderNaoDeveRetornarElementos()
    {
        //Arrange        
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), _pokemonTestsFixture.GerarPokemonResponseSemStats(), null);

        var pokemonDetailModel = _pokemonTestsFixture.GerarPokemonModelValido();
        pokemonDetailModel.NiveisDePoder = new Dictionary<string, long>();

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);

        _pokemonTestsFixture.Mapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailModel);

        if (response.Content.Stats.Any())
        {
            pokemonDetailModel.NiveisDePoder = _pokemonTestsFixture.GerarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var resultadoObtido = await _pokemonService.ObterPokemonPorId(1);

        //Assert                 
        resultadoObtido.NiveisDePoder.Should().HaveCount(0);
    }

    [Fact(DisplayName = "Retornar NiveisDePoder baseado na lista de Stats")]
    [Trait("Categoria", "ObterPokemonPorId")]
    public async Task ObterPokemonPorId_StatsRetornarElementos_NiveisDePoderDeveRetornarElementos()
    {
        //Arrange        
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), _pokemonTestsFixture.GerarPokemonResponseValido(), null, null);

        var pokemonDetailModelFaker = _pokemonTestsFixture.GerarPokemonModelValido();
        pokemonDetailModelFaker.NiveisDePoder = new Dictionary<string, long>();

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);

        _pokemonTestsFixture.Mapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailModelFaker);

        if (response.Content.Stats.Any())
        {
            pokemonDetailModelFaker.NiveisDePoder = _pokemonTestsFixture.GerarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var resultado = await _pokemonService.ObterPokemonPorId(1);

        //Assert                
        resultado.NiveisDePoder.Count.Should().Be(pokemonDetailModelFaker.NiveisDePoder.Count);

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
        niveisPoder?.Keys.Should().NotContain("max-stat");

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
        resultadoObtido.Should().Be(resultadoEsperado);
    }   

    [Fact(DisplayName = "Retornar PokemonListModel")]
    [Trait("Categoria", "ObterTodosPokemons")]
    public async Task ObterTodosPokemons_HttpStatusCodeOK_DeveRetornarPokemonListModel()
    {
        //Arrange
        var pokeList = _pokemonTestsFixture.GerarPokeListValido(10);

        var pokeListModel = _pokemonTestsFixture.GerarPokemonListModel(pokeList);

        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.OK), pokeList, null);

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterTodosPokemons()).ReturnsAsync(response);

        _pokemonTestsFixture._AspNetData.Setup(x => x.AddressObterPokemonPorId).Returns(POKEMON_BASE_URL);

        if (!response.IsSuccessStatusCode)
        {
            _pokemonTestsFixture.Notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));
            
        }
        _pokemonTestsFixture.Mapper.Setup(x => x.Map<PokemonListModel>(response.Content)).Returns(pokeListModel);

        //Act
        var resultadoObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        resultadoObtido.Should().NotBeNull();        
    }

    [Fact(DisplayName = "Não deve haver notificação")]
    [Trait("Categoria", "ObterTodosPokemons")]
    public async Task ObterTodosPokemons_HttpStatusCodeOK__NaoDeveNotificar()
    {
        //Arrange
        var pokeList = _pokemonTestsFixture.GerarPokeListValido(10);

        var pokeListModel = _pokemonTestsFixture.GerarPokemonListModel(pokeList);

        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.OK), pokeList, null);

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterTodosPokemons()).ReturnsAsync(response);

        _pokemonTestsFixture._AspNetData.Setup(x => x.AddressObterPokemonPorId).Returns(POKEMON_BASE_URL);

        if (response.IsSuccessStatusCode)
        {
            _pokemonTestsFixture.Notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));

        }
        _pokemonTestsFixture.Mapper.Setup(x => x.Map<PokemonListModel>(response.Content)).Returns(pokeListModel);

        //Act
        var resultadoObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        _pokemonTestsFixture.Notificador.Verify(x => x.Notificar(It.IsAny<Notificacao>()), Times.Never);
    }

    [Fact(DisplayName ="Notificar com mensagem empty")]
    [Trait("Categoria", "ObterTodosPokemons")]
    public void ObterTodosPokemons_NotificarMensagemStringEmpty_DeveChamarNotificador()
    {
        //Arrange
        var pokemonList = new PokemonListModel();

        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonTestsFixture.PokemonApi.Setup(x => x.ObterTodosPokemons()).ReturnsAsync(response);      
        
        if (!response.IsSuccessStatusCode)
        {
            _pokemonService.Notificar("");
            _pokemonTestsFixture.Notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));
            pokemonList = new PokemonListModel();
        }

        //Act        
        

        //Assert        
        _pokemonTestsFixture.Notificador.Verify(x => x.Notificar(It.IsAny<Notificacao>()));


    }

    [Fact(DisplayName ="Exception ao receber uma lista vazia")]
    [Trait("Categoria", "ObterPokemons")]
    public void ObterPokemons_PokemonListModelVazio_FalhaArgumentNullException()
    {
        //Arrange        
        var pokemonList = new PokemonListModel();
        
        //Act         
        Action act = () => _pokemonService.ObterPokemons(pokemonList);

        //Assert        
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName ="Sucesso ao recuperar a List")]
    [Trait("Categoria", "ObterPokemons")]
    public void ObterPokemons_PokemonListModelComValor_RetornarListPokemonBaseEquivalente()
    {
        //Arrange        
        var pokeList = _pokemonTestsFixture.GerarPokeListValido(10);
        var pokemonList = _pokemonTestsFixture.GerarPokemonListModel(pokeList);
        _pokemonTestsFixture._AspNetData.Setup(x => x.AddressObterPokemonPorId).Returns("https://meusite.com.br/");

        //Act 
        var resultado = _pokemonService.ObterPokemons(pokemonList);


        //Assert
        resultado.Should().HaveCount(10);
    }  
}

