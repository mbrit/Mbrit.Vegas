using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameProjectionDto
    {
        [JsonPropertyName("outcomes")]
        public List<WalkGameProjectionItemDto> Outcomes { get; set; } = new List<WalkGameProjectionItemDto>();
    }
}
