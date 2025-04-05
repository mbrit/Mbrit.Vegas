using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class PingResponse
    {
        [JsonPropertyName("dtUtc")]
        public DateTime DtUtc { get; set; }
    }
}
