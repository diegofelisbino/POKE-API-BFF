using Pokedex.Domain.Interfaces;

namespace Pokedex.Api.Extensions
{
    public class AspNetData : IAspNetData
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetData(IHttpContextAccessor accessor)
        {
            _accessor = accessor; 
        }

        public string AddressObterPokemonPorId => ($"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host.Value}{_accessor.HttpContext.Request.Path.Value}");
    }
}
