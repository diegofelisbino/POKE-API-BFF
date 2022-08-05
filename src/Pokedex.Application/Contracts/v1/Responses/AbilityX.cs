namespace Pokedex.Application.Contracts.v1.Responses
{   
    using System.Text.Json.Serialization;

    public record AbilityX
    {
        [JsonPropertyName("ability")]
        public Species Ability { get; set; }

        [JsonPropertyName("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonPropertyName("slot")]
        public long Slot { get; set; }
    }
}
