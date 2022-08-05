namespace Pokedex.Application.Contracts.v1.Responses
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;

    public record MoveX
    {
        [JsonPropertyName("move")]
        public Species Move { get; set; }

        [JsonPropertyName("version_group_details")]
        public List<VersionGroupDetail> VersionGroupDetails { get; set; }
    }
}
