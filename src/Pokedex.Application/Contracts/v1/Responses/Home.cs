namespace Pokedex.Application.Contracts.v1.Responses
{
    using System;
    using System.Text.Json.Serialization;

    public record Home
    {
        [JsonPropertyName("front_default")]
        public Uri FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public Uri FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public Uri FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public Uri FrontShinyFemale { get; set; }
    }
}
