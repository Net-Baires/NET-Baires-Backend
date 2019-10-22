using Newtonsoft.Json;

namespace NetBaires.Api.Models.ServicesResponse.Badgr
{
    public partial class Status
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}