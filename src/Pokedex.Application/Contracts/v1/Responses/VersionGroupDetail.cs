namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record VersionGroupDetail
    {
        [JsonPropertyName("level_learned_at")]
        public long LevelLearnedAt { get; set; }

        [JsonPropertyName("move_learn_method")]
        public Species MoveLearnMethod { get; set; }

        [JsonPropertyName("version_group")]
        public Species VersionGroup { get; set; }
    }
}
