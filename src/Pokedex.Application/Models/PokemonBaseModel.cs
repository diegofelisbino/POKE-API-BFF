
using Pokedex.Application.Configurations;

namespace Pokedex.Application.Models
{
    public class PokemonBaseModel
    {  
        public long Id { get; set; }
        public string Nome { get; set; }

        private Uri _imagemUrl = new Uri(Config.BASE_ADRESS_IMAGEM);
        public Uri ImagemUrl
        {
            get
            {
                string uri = $"{_imagemUrl.ToString()}{this.Id.ToString()}.png";
                _imagemUrl = new Uri(uri);
                return _imagemUrl;
            }
        }       
        public Uri Url { get; set; }

    }
}
