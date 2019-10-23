using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class TopLeft
    {
        [JsonProperty("x")]
        public long X { get; set; }

        [JsonProperty("y")]
        public long Y { get; set; }
    }
}