using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Models.ServicesResponse.Badgr
{
    public class Assertion
    {
        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("openBadgeId")]
        public Uri OpenBadgeId { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("badgeclass")]
        public string Badgeclass { get; set; }

        [JsonProperty("badgeclassOpenBadgeId")]
        public Uri BadgeclassOpenBadgeId { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("issuerOpenBadgeId")]
        public Uri IssuerOpenBadgeId { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }

        [JsonProperty("issuedOn")]
        public DateTimeOffset IssuedOn { get; set; }

        [JsonProperty("narrative")]
        public string Narrative { get; set; }

        [JsonProperty("evidence")]
        public object[] Evidence { get; set; }

        [JsonProperty("revoked")]
        public bool Revoked { get; set; }

        [JsonProperty("revocationReason")]
        public object RevocationReason { get; set; }

        [JsonProperty("acceptance")]
        public string Acceptance { get; set; }

        [JsonProperty("expires")]
        public object Expires { get; set; }

        [JsonProperty("extensions")]
        public Extensions Extensions { get; set; }
    }
}