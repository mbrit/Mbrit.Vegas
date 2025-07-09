using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class WalkOutcomesBucketDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("majorBustPercentage")]
        public decimal MajorBustPercentage { get; set; }

        [JsonPropertyName("minorBustPercentage")]
        public decimal MinorBustPercentage { get; set; }

        [JsonPropertyName("evensPercentage")]
        public decimal EvensPercentage { get; set; }

        [JsonPropertyName("spike0p5Percentage")]
        public decimal Spike0p5Percentage { get; set; }

        [JsonPropertyName("spike1Percentage")]
        public decimal Spike1Percentage { get; set; }

        [JsonPropertyName("spike1PlusPercentage")]
        public decimal Spike1PlusPercentage { get; set; }

        [JsonPropertyName("averageProfitWhenWon")]
        public int AverageProfitWhenWon { get; set; }
    }
}
