
using AutoMapper;
using FluentAssertions;
using Moq;
using Pokedex.Application.Contracts;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Models;
using Pokedex.Application.Notificacoes;
using Pokedex.Application.Services;
using Pokedex.Test.Mocks;
using Refit;
using System.Net;

namespace Pokedex.Test.UnitTests.PokemonServiceTest;

public class PokemonServiceTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPokemonApi> _pokemonApi;
    private readonly Mock<INotificador> _notificador;
    private PokemonService _pokemonService;

    const string MSG_FALHA_COMUNICACAO = "Falhaa na comunicação com a Api Principal";

    public PokemonServiceTest()
    {
        _mockMapper = new Mock<IMapper>();
        _pokemonApi = new Mock<IPokemonApi>();
        _notificador = new Mock<INotificador>();

        _pokemonService = new PokemonService(_pokemonApi.Object, _mockMapper.Object, _notificador.Object);
    }

    [Fact]
    public async Task ObterPokemonPorId_Quando_Response_Sucesso()
    {
        //Arrange
        var pokemonMock = PokemonResponseMock.ObterPikachuResponseMock();

        var resultadoEsperado = PokemonDetailModelMock.ObterPikachuModelMock();

        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), pokemonMock, null);

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mockMapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(resultadoEsperado);

        if (!response.IsSuccessStatusCode)
        {
            _notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));
            resultadoEsperado = new PokemonDetailModel();
        }

        //Act            
        var resultadoObtido = await _pokemonService.ObterPokemonPorId(25);

        //Assert
        resultadoObtido.Should().NotBeNull();
        resultadoObtido.Should().BeEquivalentTo(resultadoEsperado);
    }


    [Fact]
    public async Task ObterPokemonPorId_Quando_Response_Not_Success()
    {
        //Arrange        
        
        var responseMock = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(responseMock);


        //Act                
        var pokemonObtido = await _pokemonService.ObterPokemonPorId(25);

        var notificador = new Notificador();
        notificador.Notificar(new Notificacao(MSG_FALHA_COMUNICACAO));
        bool temNotificacao = notificador.TemNotificacao();
        var notificadoesObtidas = notificador.ObterNotificacoes().First();

        //Assert
        pokemonObtido.Should().BeEquivalentTo(new PokemonDetailModel());
        temNotificacao.Should().BeTrue();        
        notificadoesObtidas.Should().BeEquivalentTo(new Notificacao(MSG_FALHA_COMUNICACAO));
        notificadoesObtidas.Mensagem.Should().Be(MSG_FALHA_COMUNICACAO);
        notificadoesObtidas.Mensagem.Should().NotBeEmpty();
        notificadoesObtidas.Mensagem.Should().NotBeNullOrWhiteSpace();
        notificadoesObtidas.Mensagem.Should().NotBe("");
    }

    [Fact]
    public async Task ObterPokemonPorId_Quando_Response_Not_Success_()
    {
        //Arrange        

        var responseMock = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(responseMock);


        //Act                
        var pokemonObtido = await _pokemonService.ObterPokemonPorId(25);

        var notificador = new Notificador();
        notificador.Notificar(new Notificacao(""));
        bool temNotificacao = notificador.TemNotificacao();
        var notificadoesObtidas = notificador.ObterNotificacoes().First();

        //Assert
        pokemonObtido.Should().BeEquivalentTo(new PokemonDetailModel());
        temNotificacao.Should().BeTrue();        
        notificadoesObtidas.Mensagem.Should().Be("");
    }

    [Fact]
    public async Task ObterTodosPokemons_Quando_Response_Sucesso()
    {
        //Arrange
        var pokeListMock = PokemonResponseMock.ObterTodosPokemonsResponse(10);

        var pokeListModelMock = PokemonListModelMock.ConverterPokeListEmPokeListModel(pokeListMock);

        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.OK), pokeListMock, null);

        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);
        _mockMapper.Setup(x => x.Map<PokemonListModel>(response.Content)).Returns(pokeListModelMock);

        if (!response.IsSuccessStatusCode)
        {
            _notificador.Setup(x => x.Notificar(It.IsAny<Notificacao>()));
        }

        //Act
        var resultadoObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        resultadoObtido.Should().NotBeNull();


    }

    [Fact]
    public async Task ObterTodosPokemon_Quando_Response_Not_Success()
    {
        //Arrange                
        var responseMock = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, null);

        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(responseMock);

        

        //Act                
        var pokemonObtido = await _pokemonService.ObterTodosPokemons();

        var notificador = new Notificador();
        notificador.Notificar(new Notificacao(MSG_FALHA_COMUNICACAO));
        bool temNotificacao = notificador.TemNotificacao();
        var notificadoesObtidas = notificador.ObterNotificacoes().First();

        //Assert
        pokemonObtido.Should().BeEquivalentTo(new PokemonListModel());
        temNotificacao.Should().BeTrue();
        notificadoesObtidas.Should().BeEquivalentTo(new Notificacao(MSG_FALHA_COMUNICACAO));
        notificadoesObtidas.Mensagem.Should().Be(MSG_FALHA_COMUNICACAO);
        notificadoesObtidas.Mensagem.Should().NotBeEmpty();
        notificadoesObtidas.Mensagem.Should().NotBeNullOrWhiteSpace();
        notificadoesObtidas.Mensagem.Should().NotBe("");
    }


    [Fact]
    public async Task ObterPokemonPorId_Quando_Stats_Nao_Retornar_Elementos_Entao_NiveisDePoder_Nao_Deve_Retornar_Elementos()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        pokemonResponseMock.Stats = new List<StatX>();

        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), pokemonResponseMock, null, null);

        var pokemonDetailMock = PokemonDetailModelMock.ObterPikachuModelMock();
        pokemonDetailMock.NiveisDePoder = new Dictionary<string, long>();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mockMapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailMock);

        if (response.Content.Stats.Count >= 1)
        {
            pokemonDetailMock.NiveisDePoder = PokemonDetailModelMock.RecuperarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var pokemonDetail = await _pokemonService.ObterPokemonPorId(25);

        int resultadoObtido = pokemonDetail.NiveisDePoder.Count;
        int resultadoEsperado = 0;

        //Assert         
        resultadoObtido.Should().Be(resultadoEsperado);
    }

    [Fact]
    public async Task ObterPokemonPorId_Quando_Stats_Retornar_Elementos_Entao_NiveisDePoder_Deve_Retornar_Elementos()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), pokemonResponseMock, null, null);

        var pokemonDetailMock = PokemonDetailModelMock.ObterPikachuModelMock();
        pokemonDetailMock.NiveisDePoder = new Dictionary<string, long>();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mockMapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailMock);

        if (response.Content.Stats.Count >= 1)
        {
            pokemonDetailMock.NiveisDePoder = PokemonDetailModelMock.RecuperarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var pokemonDetail = await _pokemonService.ObterPokemonPorId(25);
        int resultadoObtido = pokemonDetail.NiveisDePoder.Count;
        int resultadoEsperado = 7;

        //Assert         
        resultadoObtido.Should().Be(resultadoEsperado);
    }

    [Fact]
    public async Task ObterPokemonPorId_Quando_Stats_Retornar_1_Elemento_Entao_NiveisDePoder_Deve_Retornar_2_Elementos()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        var stat = pokemonResponseMock.Stats.Take(1).FirstOrDefault();
        pokemonResponseMock.Stats.Clear();
        pokemonResponseMock.Stats.Add(stat);

        var response = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), pokemonResponseMock, null, null);

        var pokemonDetailMock = PokemonDetailModelMock.ObterPikachuModelMock();
        pokemonDetailMock.NiveisDePoder = new Dictionary<string, long>();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(response);
        _mockMapper.Setup(x => x.Map<PokemonDetailModel>(response.Content)).Returns(pokemonDetailMock);

        if (response.Content.Stats.Count >= 1)
        {
            pokemonDetailMock.NiveisDePoder = PokemonDetailModelMock.RecuperarNiveisDePoderPorStats(response.Content.Stats);
        }

        //Act                       
        var pokemonDetail = await _pokemonService.ObterPokemonPorId(25);
        int resultadoObtido = pokemonDetail.NiveisDePoder.Count;
        int resultadoEsperado = 1;

        //Assert         
        resultadoObtido.Should().Be(resultadoEsperado);
    }

    [Fact]
    public void ObterPokemonPorId_Quando_RecuperarNiveisDePoderPorStats_Nao_Retornar_Elementos_Entao_Deve_Retornar_Null()
    {
        //Arrange
        var listaStatsVazia = new List<StatX>();

        //Act
        var resultadoObtido = _pokemonService.RecuperarNiveisDePoderPorStats(listaStatsVazia);
        var resultadoEsperado = new Dictionary<string, long>();

        //Assert
        Assert.Equal(resultadoEsperado, resultadoObtido);
    }

    [Fact]
    public void ObterPokemonPorId_Quando_RecuperarNiveisDePoderPorStats_Nao_Receber_Elementos_Entao_Resultado_Deve_Conter_MaxStat()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        var listaStats = pokemonResponseMock.Stats;

        //Act
        var niveisPoder = _pokemonService.RecuperarNiveisDePoderPorStats(listaStats);

        bool resultadoObtido = niveisPoder.Keys.Contains("max-stat");
        bool resultadoEsperado = true;


        //Assert
        Assert.Equal(resultadoEsperado, resultadoObtido);
    }

    [Fact]
    public void ObterPokemonPorId_Quando_RecuperarNiveisDePoderPorStats_Receber_1_Elemento_Entao_Resultado_Nao_Deve_Retornar_MaxStat()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        var listaStats = pokemonResponseMock.Stats.Take(1).ToList();

        //Act
        var niveisPoder = _pokemonService.RecuperarNiveisDePoderPorStats(listaStats);

        bool resultadoObtido = niveisPoder.Keys.Contains("max-stat");
        bool resultadoEsperado = false;


        //Assert
        Assert.Equal(resultadoEsperado, resultadoObtido);
    }

    [Fact]
    public void ObterPokemonPorId_Quando_Possuir_MaxBaseStat_Entao_Deve_Atribuir_Maior_Valor_Stats()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        var stats = pokemonResponseMock.Stats;

        long resultadoEsperado = stats.Select(s => s.BaseStat).Max();

        //Act
        var niveisPoder = _pokemonService.RecuperarNiveisDePoderPorStats(stats);

        long resultadoObtido = niveisPoder.Where(x => x.Key == "max-stat").Select(x => x.Value).First();

        //Assert
        Assert.Equal(resultadoEsperado, resultadoObtido);
    }


    [Fact]
    public void ObterTodosPokemons_Quando_Results_Receber_1_Elemento_Entao_Pokemons_Deve_Implementar_1_Elemento()
    {
        //Arrange
        var resultMock = new Results { Name = "bulbasaur", Url = new Uri("https://pokeapi.co/api/v2/pokemon/1/") };
        var resultsMock = new List<Results>();
        resultsMock.Add(resultMock);

        int valorEsperado = 0;

        if (resultsMock.Count >= 1)
        {
            valorEsperado++;
        }

        //Act
        var pokemonListModel = new PokemonListModel();
        pokemonListModel.Results = resultsMock;

        int valorObtido = pokemonListModel.Pokemons.Count();

        //Assert
        valorEsperado.Should().Be(valorObtido);

    }

    [Fact]
    public void ObterTodosPokemons_Quando_Nao_Receber_Results_Entao_Pokemons_Nao_Deve_Implementar_List()
    {
        //Arrange        
        var resultsMock = new List<Results>();

        int valorEsperado = 0;

        if (resultsMock.Count >= 1)
        {
            valorEsperado++;
        }


        //Act
        var pokemonListModel = new PokemonListModel();
        pokemonListModel.Results = resultsMock;

        int valorObtido = pokemonListModel.Pokemons.Count();


        //Assert
        valorEsperado.Should().Be(valorObtido);
    }

}

