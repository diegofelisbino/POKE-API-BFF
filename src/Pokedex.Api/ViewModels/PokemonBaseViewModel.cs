using Pokedex.Api.Configurations;

namespace Pokedex.Api.ViewModels
{
    public record PokemonBaseViewModel
    {

        private long _id;
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Nome { get; set; }
        public Uri ImagemUrl { get; set; }

        private Uri _url = new Uri($"{ApiConfig.MEU_ENDERECO_BASE}{ApiConfig.MINHA_ROTA_BASE_V1}");
        public Uri Url
        {
            get
            {
                _url = new Uri($"{_url}{_id}");
                return _url;
            }

        }

    }
}






