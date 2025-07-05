using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    public class PredictRequest
    {
        [JsonPropertyName("label")]
        public int Label { get; set; }

        [JsonPropertyName("vector")]
        public List<int> Vector { get; set; } = new List<int>();
    }
}
