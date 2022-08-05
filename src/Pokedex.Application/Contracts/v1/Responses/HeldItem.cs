namespace Pokedex.Application.Contracts.v1.Responses
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public record HeldItem
    {
        [JsonPropertyName("item")]
        public Species Item { get; set; }

        [JsonPropertyName("version_details")]
        public List<VersionDetail> VersionDetails { get; set; }
    }
}
