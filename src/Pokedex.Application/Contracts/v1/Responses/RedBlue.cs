namespace Pokedex.Application.Contracts.v1.Responses
{
    using System;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public record RedBlue
    {
        [JsonPropertyName("back_default")]
        public Uri BackDefault { get; set; }

        [JsonPropertyName("back_gray")]
        public Uri BackGray { get; set; }

        [JsonPropertyName("back_transparent")]
        public Uri BackTransparent { get; set; }

        [JsonPropertyName("front_default")]
        public Uri FrontDefault { get; set; }

        [JsonPropertyName("front_gray")]
        public Uri FrontGray { get; set; }

        [JsonPropertyName("front_transparent")]
        public Uri FrontTransparent { get; set; }
    }
}
