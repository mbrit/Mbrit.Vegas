
using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameActionsDto
    {
        [JsonPropertyName("instructions")]
        public string Instructions { get; set; }

        [JsonPropertyName("canPlay")]
        public bool CanPlay { get; set; }

        [JsonPropertyName("play")]
        public int Play { get; set; }

        [JsonPropertyName("playUnits")]
        public int PlayUnits { get; set; }

        [JsonPropertyName("canHailMary")]
        public bool CanHailMary { get; set; }

        [JsonPropertyName("hailMary")]
        public int HailMarys { get; set; }

        [JsonPropertyName("hailMaryUnits")]
        public int HailMaryUnits { get; set; }

        [JsonPropertyName("canWalk")]
        public bool CanWalk { get; set; }

        internal void TouchCurrency(int unit) => this.Play = this.PlayUnits * unit;
    }
}
