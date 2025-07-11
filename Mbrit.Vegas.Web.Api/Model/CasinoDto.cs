using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class CasinoDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public LocationDto Location { get; set; }
    }
}