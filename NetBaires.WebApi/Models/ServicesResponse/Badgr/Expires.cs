using Newtonsoft.Json;

namespace NetBaires.Api.Models.ServicesResponse.Badgr
{
    public  class Expires
    {
        [JsonProperty("amount")]
        public object Amount { get; set; }

        [JsonProperty("duration")]
        public object Duration { get; set; }
    }
}