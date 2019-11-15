using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Auth
{
    public partial class TokenToReportMyAttendance
    {
        [JsonProperty("exp")]
        [JsonConverter(typeof(NumericDate))]
        public DateTime Exp { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}