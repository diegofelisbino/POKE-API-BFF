namespace Pokedex.Application.Contracts.v1.Responses
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public record Versions
    {
        [JsonPropertyName("generation-i")]
        public GenerationI GenerationI { get; set; }

        [JsonPropertyName("generation-ii")]
        public GenerationII GenerationII { get; set; }

        [JsonPropertyName("generation-iii")]
        public GenerationIII GenerationIII { get; set; }

        [JsonPropertyName("generation-iv")]
        public GenerationIV GenerationIV { get; set; }

        [JsonPropertyName("generation-v")]
        public GenerationV GenerationV { get; set; }

        [JsonPropertyName("generation-vi")]
        public Dictionary<string, Home> GenerationVI { get; set; }

        [JsonPropertyName("generation-vii")]
        public GenerationVII GenerationVII { get; set; }

        [JsonPropertyName("generation-viii")]
        public GenerationVIII GenerationVIII { get; set; }
    }
}
