using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    public class PredictResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("prediction")]
        public int Prediction { get; set; }

        [JsonPropertyName("probabilities")]
        public List<decimal> Probabilities { get; set; } = new List<decimal>();
    }
}
