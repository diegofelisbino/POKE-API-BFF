namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record TypeElement
    {
        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        [JsonPropertyName("type")]
        public Species Type { get; set; }
    }
}
