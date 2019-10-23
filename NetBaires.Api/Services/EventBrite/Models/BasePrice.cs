using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class BasePrice
    {
        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("major_value")]
        public string MajorValue { get; set; }
    }
}