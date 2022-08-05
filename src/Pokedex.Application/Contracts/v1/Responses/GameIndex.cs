namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record GameIndex
    {
        [JsonPropertyName("game_index")]
        public long Index { get; set; }

        [JsonPropertyName("version")]
        public Species Version { get; set; }
    }
}
