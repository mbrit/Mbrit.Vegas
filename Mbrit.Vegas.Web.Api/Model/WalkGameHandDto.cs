using Mbrit.Vegas.Simulator;
using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkGameHandDto
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("isDraft")]
        public bool IsDraft { get; set; }

        [JsonPropertyName("needsAnswer")]
        public bool NeedsAnswer { get; set; }

        [JsonPropertyName("casino")]
        public CasinoDto Casino { get; set; }

        [JsonPropertyName("game")]
        public GameItemDto Game { get; set; }

        [JsonPropertyName("pilesBefore")]
        public WalkGamePilesDto PilesBefore { get; set; }

        [JsonPropertyName("hasSeenSpike0p5")]
        public bool HasSeenSpike0p5 { get; set; }

        [JsonPropertyName("isOverSpike0p5")]
        public bool IsOverSpike0p5 { get; set; }

        [JsonPropertyName("hasSeenSpike1")]
        public bool HasSeenSpike1 { get; set; }

        [JsonPropertyName("isOverSpike1")]
        public bool IsOverSpike1 { get; set; }

        [JsonPropertyName("actions")]
        public WalkGameActionsDto Actions { get; set; }

        [JsonPropertyName("hasActions")]
        public bool HasActions => this.Actions != null;

        [JsonPropertyName("action")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WalkGameAction Action { get; set; }

        [JsonPropertyName("probabilitySpace")]
        public WalkGameProbabilitySpaceDto ProbabilitySpace { get; set; }

        [JsonPropertyName("hasProbabilitySpace")]
        public bool HasProbabilitySpace => this.ProbabilitySpace != null;

        [JsonPropertyName("probabilitySpaceAvailableAt")]
        public int ProbabilitySpaceAvailableAt { get; set; }
    }
}
