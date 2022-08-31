using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Interfaces;
using Pokedex.Test.Mocks;
using FluentAssertions;


namespace Pokedex.Test.IntegrationTests
{
    [CollectionDefinition(nameof(PokemonCollection))]
    public class PokemonServiceTest : IClassFixture<PokemonTestsFixture>
    {

        private readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;
        private readonly PokemonTestsFixture _pokemonTestsFixture;

        public PokemonServiceTest(PokemonTestsFixture pokemonTestsFixture)
        {
            var services = DependecyInjectionConfig.ResolverDependencias();

            var provider = services.BuildServiceProvider();
            _pokemonService = provider.GetService<IPokemonService>();
            _pokemonTestsFixture = pokemonTestsFixture;
        }        

        [Theory]
        [InlineData(25)]
        public async Task Quando_ObterPokemonPorId_Receber_Id_25_RetornaPikachu(long idPokemon)
        {
            //Arrange
            var response = _pokemonTestsFixture.GerarPokemonResponseValido();
            var pokemonEsperado = _pokemonTestsFixture.GerarPokemonModelValido();
            pokemonEsperado.NiveisDePoder = _pokemonTestsFixture.GerarNiveisDePoderPorStats(response.Stats);

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

            var response = _pokemonTestsFixture.GerarPokeListValido(quantidade);

            var model = _pokemonTestsFixture.GerarPokemonListModel(response);

            int esperado = model.Pokemons.Count;

            //Act
            var pokemonList = await _pokemonService.ObterTodosPokemons();

            int obtido = pokemonList.Pokemons.Take(quantidade).Count();

            //Assert
            Assert.Equal(esperado, obtido);
        }

    }

}
