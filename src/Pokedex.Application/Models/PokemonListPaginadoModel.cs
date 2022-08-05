namespace Pokedex.Application.Models
{
    public class PokemonListPaginadoModel : PokemonListModel
    {
        public int Quantidade { get; set; }

        private Uri _proximo;
        public Uri Proximo
        {
            get
            {
                //_proximo = (ProximoOffSet < 0 || ApiConfig.Limit < 0) ? null : new Uri($"{ApiConfig.MEU_ENDERECO_BASE}{ApiConfig.MINHA_ROTA_BASE}?offset={this.ProximoOffSet}&limit={ApiConfig.Limit.ToString()}");
                return _proximo;
            }
        }

        private Uri _anterior;
        public Uri Anterior
        {
            get
            {
                //_anterior = (AnteriorOffSet < 0 || ApiConfig.Limit < 0) ? null : new Uri($"{ApiConfig.MEU_ENDERECO_BASE}{ApiConfig.MINHA_ROTA_BASE}?offset={this.AnteriorOffSet}&limit={ApiConfig.Limit.ToString()}");

                return _anterior;
            }
        }

        
        private int ProximoOffSet { get; set; }
        private int AnteriorOffSet { get; set; }       

        public PokemonListPaginadoModel()
        {
            this.IncrementarOffSet();
            this.DecrementarOffSet();
        }

        private void IncrementarOffSet()
        {
            //ProximoOffSet = ApiConfig.OffSet + ApiConfig.Limit;
        }

        private void DecrementarOffSet()
        {
            //AnteriorOffSet = ApiConfig.OffSet - ApiConfig.Limit;

        }

    }

    



}
