using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Auth.Tokens
{
    public partial class TokenToReportGeneralAttendance
    {
        [JsonProperty("exp")]
        [JsonConverter(typeof(NumericDate))]
        public DateTime Exp { get; set; }
        public int EventId { get; set; }
    }
}