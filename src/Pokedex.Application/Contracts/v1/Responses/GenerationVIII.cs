namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record GenerationVIII
    {
        [JsonPropertyName("icons")]
        public DreamWorld Icons { get; set; }
    }
}
