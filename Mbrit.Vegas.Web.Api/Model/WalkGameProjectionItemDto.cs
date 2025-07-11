using Mbrit.Vegas.Simulator;
using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameProjectionItemDto 
    {
        [JsonPropertyName("mode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WalkGameMode Mode { get; set; }

        [JsonPropertyName("outcomes")]
        public WalkOutcomesBucketDto Outcomes { get; set; }
    }
}