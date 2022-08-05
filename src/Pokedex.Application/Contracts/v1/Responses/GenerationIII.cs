namespace Pokedex.Application.Contracts.v1.Responses
{
    using Newtonsoft.Json;
    using System.Text.Json.Serialization;

    public record GenerationIII
    {
        [JsonPropertyName("emerald")]
        public Emerald Emerald { get; set; }

        [JsonPropertyName("firered-leafgreen")]
        public Gold FireredLeafgreen { get; set; }

        [JsonPropertyName("ruby-sapphire")]
        public Gold RubySapphire { get; set; }
    }
}
