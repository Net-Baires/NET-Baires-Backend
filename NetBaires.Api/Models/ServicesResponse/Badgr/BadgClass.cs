using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Models.ServicesResponse.Badgr
{
    public  class BadgClass
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

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("issuerOpenBadgeId")]
        public Uri IssuerOpenBadgeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("criteriaUrl")]
        public Uri CriteriaUrl { get; set; }

        [JsonProperty("criteriaNarrative")]
        public string CriteriaNarrative { get; set; }

        [JsonProperty("alignments")]
        public object[] Alignments { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("expires")]
        public Expires Expires { get; set; }

        [JsonProperty("extensions")]
        public Extensions Extensions { get; set; }
    }
}