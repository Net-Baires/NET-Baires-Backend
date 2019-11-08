using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Description
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }
}