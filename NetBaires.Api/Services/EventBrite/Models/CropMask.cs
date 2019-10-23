using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class CropMask
    {
        [JsonProperty("top_left")]
        public TopLeft TopLeft { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}