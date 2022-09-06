namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record StatX
    {
        [JsonPropertyName("base_stat")]
        public long BaseStat { get; set; }       

        [JsonPropertyName("stat")]
        public Species Stat { get; set; }
    }
}
