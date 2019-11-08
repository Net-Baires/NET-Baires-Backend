using Newtonsoft.Json;

namespace NetBaires.Api.Services.BadGr
{
    public class Recipient
    {
        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("hashed")]
        public bool Hashed { get; set; }

        [JsonProperty("plaintextIdentity")]
        public string PlaintextIdentity { get; set; }
    }
}