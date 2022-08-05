namespace Pokedex.Application.Contracts.v1.Responses
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    

    public record PokeList
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("next")]
        public Uri Next { get; set; }

        [JsonPropertyName("previous")]
        public Uri Previous { get; set; }

        [JsonPropertyName("results")]
        public List<Results> Results { get; set; }
    }

    public partial class Results
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }
    }


}
