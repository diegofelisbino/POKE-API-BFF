namespace Pokedex.Application.Contracts.v1.Responses
{
    using System;
    using System.Text.Json.Serialization;

    public record Species
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
