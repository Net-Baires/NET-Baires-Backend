using Newtonsoft.Json;

namespace NetBaires.Api.Services.BadGr
{
    public class AssertionToUser
    {
        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("badgeclass")]
        public string Badgeclass { get; set; }

        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }
    }
}