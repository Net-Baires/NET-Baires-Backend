using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Profile
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("addresses")]
        public dynamic Addresses { get; set; }
    }
}