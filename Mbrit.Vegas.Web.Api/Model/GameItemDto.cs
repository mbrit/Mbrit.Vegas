using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class GameItemDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("houseEdge")]
        public decimal HouseEdge { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }
}