using Mbrit.Vegas.Simulator;
using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameSetupRequest
    {
        [JsonPropertyName("unit")]
        public int Unit { get; set; }

        [JsonPropertyName("mode")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WalkGameMode Mode { get; set; }

        [JsonPropertyName("hailMaryCount")]
        public int HailMaryCount { get; set; }
    }
}
