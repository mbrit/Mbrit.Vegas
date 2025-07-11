using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameStateDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("piles")]
        public WalkGamePilesDto Piles { get; set; }

        [JsonPropertyName("hands")]
        public List<WalkGameHandDto> Hands { get; set; } = new List<WalkGameHandDto>();
    }
}
