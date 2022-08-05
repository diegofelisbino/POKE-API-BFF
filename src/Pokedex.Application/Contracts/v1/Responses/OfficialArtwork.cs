namespace Pokedex.Application.Contracts.v1.Responses
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public record OfficialArtwork
    {
        [JsonPropertyName("front_default")]
        public Uri FrontDefault { get; set; }
    }
}
