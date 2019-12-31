using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Auth
{
    public partial class LoginToken
    {
        [JsonProperty("exp")]
        [JsonConverter(typeof(NumericDate))]
        public DateTime Exp { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }

        public int EventId { get; set; }
    }
}