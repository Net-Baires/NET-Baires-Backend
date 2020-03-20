using System;
using Newtonsoft.Json;

namespace NetBaires.Api.Services.EventBrite.Models
{
    public class Attendee
    {

        [JsonProperty("team")]
        public object Team { get; set; }

        [JsonProperty("costs")]
        public Costs Costs { get; set; }

        [JsonProperty("resource_uri")]
        public Uri ResourceUri { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("changed")]
        public DateTimeOffset Changed { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("variant_id")]
        public object VariantId { get; set; }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("barcodes")]
        public Barcode[] Barcodes { get; set; }

        [JsonProperty("answers")]
        public Answer[] Answers { get; set; }

        [JsonProperty("checked_in")]
        public bool CheckedIn { get; set; }

        [JsonProperty("cancelled")]
        public bool Cancelled { get; set; }

        [JsonProperty("refunded")]
        public bool Refunded { get; set; }

        [JsonProperty("affiliate")]
        public string Affiliate { get; set; }

        [JsonProperty("guestlist_id")]
        public object GuestlistId { get; set; }

        [JsonProperty("invited_by")]
        public object InvitedBy { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("ticket_class_name")]
        public string TicketClassName { get; set; }

        [JsonProperty("delivery_method")]
        public string DeliveryMethod { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("order_id")]
        public long OrderId { get; set; }

        [JsonProperty("ticket_class_id")]
        public long TicketClassId { get; set; }
        [JsonProperty("checkIn")]
        public bool CheckIn { get;  set; }
    }
}