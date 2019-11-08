using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Event
    {
        [JsonProperty("name")]
        public Description Name { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("start")]
        public End Start { get; set; }

        [JsonProperty("end")]
        public End End { get; set; }

        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("changed")]
        public DateTimeOffset Changed { get; set; }

        [JsonProperty("published", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Published { get; set; }

        [JsonProperty("capacity")]
        public long Capacity { get; set; }

        [JsonProperty("capacity_is_custom")]
        public bool CapacityIsCustom { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("listed")]
        public bool Listed { get; set; }

        [JsonProperty("shareable")]
        public bool Shareable { get; set; }

        [JsonProperty("invite_only")]
        public bool InviteOnly { get; set; }

        [JsonProperty("online_event")]
        public bool OnlineEvent { get; set; }

        [JsonProperty("show_remaining")]
        public bool ShowRemaining { get; set; }

        [JsonProperty("tx_time_limit")]
        public long TxTimeLimit { get; set; }

        [JsonProperty("hide_start_date")]
        public bool HideStartDate { get; set; }

        [JsonProperty("hide_end_date")]
        public bool HideEndDate { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("is_locked")]
        public bool IsLocked { get; set; }

        [JsonProperty("privacy_setting")]
        public string PrivacySetting { get; set; }

        [JsonProperty("is_series")]
        public bool IsSeries { get; set; }

        [JsonProperty("is_series_parent")]
        public bool IsSeriesParent { get; set; }

        [JsonProperty("inventory_type")]
        public string InventoryType { get; set; }

        [JsonProperty("is_reserved_seating")]
        public bool IsReservedSeating { get; set; }

        [JsonProperty("show_pick_a_seat")]
        public bool ShowPickASeat { get; set; }

        [JsonProperty("show_seatmap_thumbnail")]
        public bool ShowSeatmapThumbnail { get; set; }

        [JsonProperty("show_colors_in_seatmap_thumbnail")]
        public bool ShowColorsInSeatmapThumbnail { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("is_free")]
        public bool IsFree { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("logo_id")]
        public long? LogoId { get; set; }

        [JsonProperty("organizer_id")]
        public string OrganizerId { get; set; }

        [JsonProperty("venue_id")]
        public long VenueId { get; set; }

        [JsonProperty("category_id")]
        public long? CategoryId { get; set; }

        [JsonProperty("subcategory_id")]
        public object SubcategoryId { get; set; }

        [JsonProperty("format_id")]
        public long? FormatId { get; set; }

        [JsonProperty("resource_uri")]
        public Uri ResourceUri { get; set; }

        [JsonProperty("is_externally_ticketed")]
        public bool IsExternallyTicketed { get; set; }

        [JsonProperty("logo")]
        public Logo Logo { get; set; }
    }
}