using System.Text.Json.Serialization;

namespace Mbrit.Vegas.Web.Api.Model
{
    public class LocationDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}