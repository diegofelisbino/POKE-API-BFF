namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record VersionDetail
    {
        [JsonPropertyName("rarity")]
        public long Rarity { get; set; }

        [JsonPropertyName("version")]
        public Species Version { get; set; }
    }
}
