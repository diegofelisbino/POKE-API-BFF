
namespace Pokedex.Api.ViewModels
{

    public record PokemonDetailViewModel 
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public Uri ImagemUri { get; set; }
        public int Peso { get; set; }
        public int Altura { get; set; }
        public List<string> Elementos { get; set; }
        public Dictionary<string, long> NiveisDePoder { get; set; }
    }
}

   


    

