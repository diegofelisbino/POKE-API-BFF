
using AutoMapper;
using FluentAssertions;
using Moq;
using Pokedex.Application.Contracts;
using Pokedex.Application.Contracts.v1.Responses;
using Pokedex.Application.Models;
using Pokedex.Application.Services;
using Pokedex.Test.Mocks;
using Refit;
using System.Linq;
using System.Net;

namespace Pokedex.Test.UnitTests.PokemonServiceTest;

public class ObterPokemonPorIdTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPokemonApi> _pokemonApi;
    private PokemonService _pokemonService;

    public ObterPokemonPorIdTest()
    {
        _mockMapper = new Mock<IMapper>();

        _pokemonApi = new Mock<IPokemonApi>();

        _pokemonService = new PokemonService(_pokemonApi.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Quando_Response_Sucesso_Deve_Retornar_PokemonDetailModel()
    {
        //Arrange
        var pokemonResponseMock = PokemonResponseMock.ObterPikachuResponseMock();
        var apiResponse = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.OK), pokemonResponseMock, null, null);

        var pokemonDetailMock = PokemonDetailModelMock.ObterPikachuModelMock();

        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(apiResponse);
        _mockMapper.Setup(x => x.Map<PokemonDetailModel>(pokemonResponseMock)).Returns(pokemonDetailMock);

        //Act            
        var valorObtido = await _pokemonService.ObterPokemonPorId(25);

        //Assert
        Assert.NotNull(valorObtido);
    }

    [Fact]
    public async Task Quando_Reseponse_404_Deve_Retornar_404()
    {
        //Arrange            
        var apiResponse = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.NotFound), null, null);
        _pokemonApi.Setup(x => x.ObterPokemonPorId(It.IsAny<long>()).Result).Returns(apiResponse);

        //Act                       
        //var exceptionObtida = await Assert.ThrowsAnyAsync<Exception>(() => _pokemonService.ObterPokemonPorId(25));
        var valorObtido = await _pokemonService.ObterPokemonPorId(25);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, valorObtido.StatusCode);

    }

    [Fact]
    public async Task Quando_Reseponse_500_Deve_Retornar_500()
    {
        //Arrange
        long idPokemon = 25;
        var apiResponse = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.InternalServerError), null, null, null);
        _pokemonApi.Setup(x => x.ObterPokemonPorId(idPokemon).Result).Returns(apiResponse);

        //Act
        var valorObtido = await _pokemonService.ObterPokemonPorId(idPokemon);

        //Assert
        Assert.Equal(HttpStatusCode.InternalServerError, valorObtido.StatusCode);
    }

    [Theory]
    [InlineData(25)]
    public async Task Quando_Response_Insuccess_Nao_Receber_404_Ou_500_Entao_Deve_Retornar_404(long idPokemon)
    {
        //Arrange     
        var apiResponse = new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.Forbidden), null, null);
        _pokemonApi.Setup(x => x.ObterPokemonPorId(idPokemon).Result).Returns(apiResponse);

        //Act
        var valorObtido = await _pokemonService.ObterPokemonPorId(idPokemon);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, valorObtido.StatusCode);
    }

    [Fact]
    public async Task Quando_Stats_Nao_Retornar_Elementos_Entao_NiveisDePoder_Nao_Deve_Retornar_Elementos()
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

        int resultadoObtido = pokemonDetail.Content.NiveisDePoder.Count;
        int resultadoEsperado = 0;

        //Assert         
        resultadoObtido.Should().Be(resultadoEsperado);
    }

    [Fact]
    public async Task Quando_Stats_Retornar_Elementos_Entao_NiveisDePoder_Deve_Retornar_Elementos()
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
        int resultadoObtido = pokemonDetail.Content.NiveisDePoder.Count;
        int resultadoEsperado = 7;

        //Assert         
        resultadoObtido.Should().Be(resultadoEsperado);
    }

    [Fact]
    public async Task Quando_Stats_Retornar_1_Elemento_Entao_NiveisDePoder_Deve_Retornar_2_Elementos()
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
        int resultadoObtido = pokemonDetail.Content.NiveisDePoder.Count;
        int resultadoEsperado = 1;

        //Assert         
        resultadoObtido.Should().Be(resultadoEsperado);
    }

    [Fact]
    public void Quando_RecuperarNiveisDePoderPorStats_Nao_Retornar_Elementos_Entao_Deve_Retornar_Null()
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
    public void Quando_RecuperarNiveisDePoderPorStats_Nao_Receber_Elementos_Entao_Resultado_Deve_Conter_MaxStat()
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
    public void Quando_RecuperarNiveisDePoderPorStats_Receber_1_Elemento_Entao_Resultado_Nao_Deve_Retornar_MaxStat()
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
    public void Quando_Possuir_MaxBaseStat_Entao_Deve_Atribuir_Maior_Valor_Stats()
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
}

public class ObterTodosPokemons
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IPokemonApi> _pokemonApi;
    private PokemonService _pokemonService;
    public ObterTodosPokemons()
    {
        _mockMapper = new Mock<IMapper>();

        _pokemonApi = new Mock<IPokemonApi>();

        _pokemonService = new PokemonService(_pokemonApi.Object, _mockMapper.Object);
    }

    #region ObterTodosPokemons  

    [Fact]
    public async Task Quando_Response_Sucesso_Entao_Deve_Retornar_PokemonListModel()
    {
        //Arrange
        var pokeListMock = PokemonResponseMock.ObterTodosPokemonsResponse(10);

        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.OK), pokeListMock, null);

        var pokeListModelMock = PokemonListModelMock.ConverterPokeListEmPokeListModel(pokeListMock);

        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);

        _mockMapper.Setup(x => x.Map<PokemonListModel>(response.Content)).Returns(pokeListModelMock);

        //Act
        var resultadoObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        Assert.NotNull(resultadoObtido.Content.Pokemons);

    }

    [Fact]
    public async Task Quando_Response_404_Entao_Deve_Retornar_404()
    {
        //Arrange
        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.NotFound), null, null);
        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);

        //Act
        var valorObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, valorObtido.StatusCode);
    }

    [Fact]
    public async Task Quando_Response_500_Entao_Deve_Retornar_500()
    {
        //Arrange            
        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.InternalServerError), null, null);

        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);

        //Act
        var valorObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        Assert.Equal(HttpStatusCode.InternalServerError, valorObtido.StatusCode);
    }

    [Fact]
    public async Task Quando_Reseponse_Insuccess_Nao_Receber_404_Ou_500_Entao_Deve_Retornar_404()
    {
        //Arrange
        var response = new ApiResponse<PokeList>(new HttpResponseMessage(HttpStatusCode.Forbidden), null, null);
        _pokemonApi.Setup(x => x.ObterTodosPokemons().Result).Returns(response);

        //Act
        var valorObtido = await _pokemonService.ObterTodosPokemons();

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, valorObtido.StatusCode);
    }

    [Fact]
    public void Quando_Results_Receber_1_Elemento_Entao_Pokemons_Deve_Implementar_1_Elemento()
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
    public void Quando_Nao_Receber_Results_Entao_Pokemons_Nao_Deve_Implementar_List()
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

    #endregion
}

