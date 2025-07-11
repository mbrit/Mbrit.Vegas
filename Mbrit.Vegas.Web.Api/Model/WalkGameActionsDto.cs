using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameActionsDto
    {
        [JsonPropertyName("canPutInAndPlay")]
        public bool CanPutInAndPlay { get; set; }

        [JsonPropertyName("canPutInUnits")]
        public int CanPutInUnits { get; set; }

        [JsonPropertyName("canPlay")]
        public bool CanPlay { get; set; }

        [JsonPropertyName("canHailMary")]
        public bool CanHailMary { get; set; }

        [JsonPropertyName("canHailMaryUnits")]
        public int CanHailMaryUnits { get; set; }

        [JsonPropertyName("canWalk")]
        public bool CanWalk { get; set; }
    }
}
