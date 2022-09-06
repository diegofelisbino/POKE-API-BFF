using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Pokedex.Test.IntegrationTests
{
    public class PokemonControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private string _url = "https://localhost:5001/api/v1/pokemon/";

        public PokemonControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;          
        }

        /*[Theory]
        [InlineData(1)]
        public async Task Quando_Get_Com_Argumento_Valido_Deve_Retornar_200(long id)
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"{_url}{id}");
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
        
        }

        [Theory]
        [InlineData("A")]
        public async Task Quando_Get_Com_Argumento_String_Deve_Retornar_404(string id)
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"{_url}{id}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(999)]
        public async Task Quando_Get_Com_Argumento_Inexistente_Deve_Retornar_400(long id)
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync($"{_url}{id}");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }*/
    }
}
