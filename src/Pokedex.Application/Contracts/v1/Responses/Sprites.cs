namespace Pokedex.Application.Contracts.v1.Responses
{   
    using System.Text.Json.Serialization;

    public record Sprites
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }

        
        
       
    }
}
