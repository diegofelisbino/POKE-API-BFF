using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Interfaces;
using Pokedex.Test.Mocks;
using FluentAssertions;


namespace Pokedex.Test.IntegrationTests
{
    public class PokemonServiceTest
    {

        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;

        public PokemonServiceTest()
        {
            var services = DependecyInjectionConfig.ResolverDependencias();

            var provider = services.BuildServiceProvider();
            _pokemonService = provider.GetService<IPokemonService>();
        }        

        [Theory]
        [InlineData(25)]
        public async Task Quando_ObterPokemonPorId_Receber_Id_25_RetornaPikachu(long idPokemon)
        {
            //Arrange
            var response = PokemonResponseMock.ObterPikachuResponseMock();
            var pokemonEsperado = PokemonDetailModelMock.ObterPikachuModelMock();
            pokemonEsperado.NiveisDePoder = PokemonDetailModelMock.RecuperarNiveisDePoderPorStats(response.Stats);

            //Act
            var pokemonObtido = await _pokemonService.ObterPokemonPorId(idPokemon);

            //Assert
            pokemonObtido.Should().BeEquivalentTo(pokemonEsperado);
           
        }

        [Fact]
        public async Task Quando_ObterTodosPokemons_ComSucesso_Retornar_Lista()
        {
            //Arrange
            int quantidade = 10;

            var response = PokemonResponseMock.ObterTodosPokemonsResponse(quantidade);

            var model = PokemonListModelMock.ConverterPokeListEmPokeListModel(response);

            int esperado = model.Pokemons.Count;

            //Act
            var pokemonList = await _pokemonService.ObterTodosPokemons();

            int obtido = pokemonList.Pokemons.Take(quantidade).Count();

            //Assert
            Assert.Equal(esperado, obtido);
        }

    }

}
