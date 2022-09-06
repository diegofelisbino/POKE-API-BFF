namespace Pokedex.Application.Contracts.v1.Responses
{   
    using System.Text.Json.Serialization;    
    public record Pokemon
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("height")]
        public int Height { get; set; }        
        
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("sprites")]
        public Sprites Sprites { get; set; }

        [JsonPropertyName("stats")]
        public List<StatX> Stats { get; set; }

        [JsonPropertyName("types")]
        public List<TypeElement> Types { get; set; }
        
    }


}
