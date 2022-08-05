using AutoMapper;
using Pokedex.Api.AutoMapper;
using Pokedex.Application.AutoMapper;

namespace Pokedex.Test.MapperTests
{
    public class MapperTest
    {
        [Fact]
        public void ConfiguracaoMappingApi_Sucesso()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ApiConfigurationMapping()));
            IMapper mapper = mapperConfiguration.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void ConfiguracaoMappingApplication_Sucesso()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationConfigurationMapping()));
            IMapper mapper = mapperConfiguration.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
