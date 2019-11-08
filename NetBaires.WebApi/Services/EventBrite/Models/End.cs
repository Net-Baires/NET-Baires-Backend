using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class End
    {
        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("local")]
        public DateTimeOffset Local { get; set; }

        [JsonProperty("utc")]
        public DateTimeOffset Utc { get; set; }
    }
}